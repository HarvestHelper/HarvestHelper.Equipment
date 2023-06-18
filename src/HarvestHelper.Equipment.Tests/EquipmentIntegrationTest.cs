

 using System.Collections.Generic;
 using System.Linq;
 using System.Net;
 using System.Net.Http;
 using System.Net.Http.Headers;
 using System.Threading.Tasks;
 using HarvestHelper.Common;
 using HarvestHelper.Equipment.Contracts;
 using HarvestHelper.Equipment.Service;
 using HarvestHelper.Equipment.Service.Controllers;
 using HarvestHelper.Equipment.Service.Dtos;
 using HarvestHelper.Equipment.Service.Entities;
 using MassTransit;
 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Mvc.Testing;
 using Microsoft.Extensions.DependencyInjection;
 using Microsoft.Extensions.Logging.Console;
 using Microsoft.Extensions.Logging;
 using Moq;
 using Xunit;

 namespace HarvestHelper.Equipment.Tests
 {
 	public class EquipmentControllerInTests : IClassFixture<WebApplicationFactory<Startup>>
 	{
 		private readonly WebApplicationFactory<Startup> _factory;

 		public EquipmentControllerInTests(WebApplicationFactory<Startup> factory)
 		{


 			_factory = factory;

 		}

 		[Fact]
 		public async Task GetAsync_ReturnsOk_WithEquipmentItems()
 		{
 			// Arrange
 			var equipmentItems = new List<EquipmentItem>
 			{
 				new EquipmentItem { Id = Guid.NewGuid(), Name = "Equipment 1" },
 				new EquipmentItem { Id = Guid.NewGuid(), Name = "Equipment 2" }
 			};
 			var mockRepository = new Mock<IRepository<EquipmentItem>>();
			var responseContent = equipmentItems;
			mockRepository.Setup(repo => repo.GetAllAsync())
 				.ReturnsAsync(equipmentItems);
 			var mockPublishEndpoint = new Mock<IPublishEndpoint>();
 			var client = _factory.WithWebHostBuilder(builder =>
 			{
 				builder.ConfigureServices(services =>
 				{
 					services.AddSingleton(mockRepository.Object);
 					services.AddSingleton(mockPublishEndpoint.Object);
 				});
 			}).CreateClient();

 			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJSUzI1NiIsImtpZCI6Ijg5MDNCODlCNzMyOTVEMjk3MEVFNzJCNDBGQTI4RTA1IiwidHlwIjoiYXQrand0In0.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MDAzIiwibmJmIjoxNjg2OTQyOTQ5LCJpYXQiOjE2ODY5NDI5NDksImV4cCI6MTY4Njk0NjU0OSwiYXVkIjpbIkVxdWlwbWVudCIsIkVxdWlwbWVudEludmVudG9yeSIsIklkZW50aXR5Il0sInNjb3BlIjpbIm9wZW5pZCIsInByb2ZpbGUiLCJlcXVpcG1lbnQuZnVsbGFjY2VzcyIsImVxdWlwbWVudEludmVudG9yeS5mdWxsYWNjZXNzIiwiSWRlbnRpdHlTZXJ2ZXJBcGkiXSwiYW1yIjpbInB3ZCJdLCJjbGllbnRfaWQiOiJwb3N0bWFuIiwic3ViIjoiYTBiNjcwNTEtZmEyZi00NmZjLTg2YjYtYTczYzU1OTg1YWE0IiwiYXV0aF90aW1lIjoxNjg2OTQyOTQ5LCJpZHAiOiJsb2NhbCIsInJvbGUiOiJBZG1pbiIsInNpZCI6IjcyNzYxQTI5NjVDRjY3NTFBQjAxRDExMTgwQTJEM0M5IiwianRpIjoiRENEMTNBRkRERkZDODFEMTRGODM0NTk3RTYxN0EwMEIifQ.gQaekLVVHJVuYL2avSTAWI9yyFm4iLiVSsYmK1iLL0osYXmJSSebbVeRYlwtItzpz_exACu8aC5LWfkcpsbGArPzRs423d-zyYtY5o0_QroRG2bYteyASlDxjvt7qbGNh8doRNtL31yBBOEd_lZF_0uLnk33QDhw5sNU1ozl8IeFjB4X1uZiOYlEu4rVICttj5gkjBRarhPmW9QC1AIaxmQ1btZMC4_tgvnJ43nDZvo_6wdMJ09gmf9sucglAoHifsNmI09MtTdydxXciDHovYuNnjYZBD5iqG-dvxQJSVavzU2IYRjqKYYGnyUfYkQ7v8DLcSuUogc4M9BVQSdTZw");
 			client.DefaultRequestHeaders.Accept.Clear();
 			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
 			client.DefaultRequestHeaders.Host = "localhost:5001";

 			 //Act
 			var response = await client.GetAsync("/equipment");

 			// Assert
 			response.EnsureSuccessStatusCode();
 			Assert.Equal(equipmentItems.Count, responseContent.Count());
 			foreach (var item in equipmentItems)
 			{
 				Assert.Contains(responseContent, i => i.Id == item.Id && i.Name == item.Name);
 			}
 		}
 	}
}