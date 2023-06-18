using Microsoft.OpenApi.Models;
using HarvestHelper.Common.MassTransit;
using HarvestHelper.Common.MongoDB;
using HarvestHelper.Equipment.Service.Entities;
using HarvestHelper.Common.Settings;
using HarvestHelper.Common.Identity;
using HarvestHelper.Common.HealthChecks;

namespace HarvestHelper.Equipment.Service
{
    public class Startup
    {
        private const string AllowedOriginSetting = "AllowedOrigin";

        private ServiceSettings serviceSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

            services.AddMongo()
                    .AddMongoRepository<EquipmentItem>("equipmentitems")
                    .AddMassTransitWithMessageBroker(Configuration)
                    .AddJwtBearerAuthentication();

            this.ConfigureAuth(services);

            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HarvestHelper.Equipment.Service", Version = "v1" });
            });

            services.AddHealthChecks()
                    .AddMongoDb();
        }

        protected virtual void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.Read, policy =>
                {
                    policy.RequireRole("Admin");
                    policy.RequireClaim("scope", "equipment.fullaccess", "equipment.readaccess");
                });

                options.AddPolicy(Policies.Write, policy =>
                {
                    policy.RequireRole("Admin");
                    policy.RequireClaim("scope", "equipment.fullaccess", "equipment.writeaccess");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HarvestHelper.Equipment.Service v1"));

                app.UseCors(builder =>
                {
                    builder.WithOrigins(Configuration[AllowedOriginSetting])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });  

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHarvestHelperHealthChecks();
            });
        }
    }
}
