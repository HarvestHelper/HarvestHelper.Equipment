

// using System.Collections.Generic;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Net.Http.Headers;
// using System.Threading.Tasks;
// using HarvestHelper.Common;
// using HarvestHelper.Equipment.Contracts;
// using HarvestHelper.Equipment.Service;
// using HarvestHelper.Equipment.Service.Controllers;
// using HarvestHelper.Equipment.Service.Dtos;
// using HarvestHelper.Equipment.Service.Entities;
// using MassTransit;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging.Console;
// using Microsoft.Extensions.Logging;
// using Moq;
// using Xunit;

// namespace HarvestHelper.Equipment.Tests
// {
// 	public class EquipmentControllerInTests : IClassFixture<WebApplicationFactory<Startup>>
// 	{
// 		private readonly WebApplicationFactory<Startup> _factory;
		
// 		public EquipmentControllerInTests(WebApplicationFactory<Startup> factory)
// 		{


// 			_factory = factory;

// 		}

// 		[Fact]
// 		public async Task GetAsync_ReturnsOk_WithEquipmentItems()
// 		{
// 			// Arrange
// 			var equipmentItems = new List<EquipmentItem>
// 			{
// 				new EquipmentItem { Id = Guid.NewGuid(), Name = "Equipment 1" },
// 				new EquipmentItem { Id = Guid.NewGuid(), Name = "Equipment 2" }
// 			};
// 			var mockRepository = new Mock<IRepository<EquipmentItem>>();
// 			mockRepository.Setup(repo => repo.GetAllAsync())
// 				.ReturnsAsync(equipmentItems);
// 			var mockPublishEndpoint = new Mock<IPublishEndpoint>();
// 			var client = _factory.WithWebHostBuilder(builder =>
// 			{
// 				builder.ConfigureServices(services =>
// 				{
// 					services.AddSingleton(mockRepository.Object);
// 					services.AddSingleton(mockPublishEndpoint.Object);
// 				});
// 			}).CreateClient();

// 			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eejtd....");
// 			client.DefaultRequestHeaders.Accept.Clear();
// 			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
// 			client.DefaultRequestHeaders.Host = "harvesthelper.westeurope.cloudapp.azure.com";

// 			// Act
// 			var response = await client.GetAsync("/equipment");

// 			// Assert
// 			response.EnsureSuccessStatusCode();
// 			//var responseContent = await response.Content.ReadAsAsync<IEnumerable<EquipmentDto>>(new[] { new JsonMediaTypeFormatter() });
// 			//Assert.Equal(equipmentItems.Count, responseContent.Count());
// 			//foreach (var item in equipmentItems)
// 			//{
// 			//	Assert.Contains(responseContent, i => i.Id == item.Id && i.Name == item.Name);
// 			//}
// 		}
// 	}
// }

/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HarvestHelper.Equipment.Service;
using HarvestHelper.Equipment.Service.Dtos;
using HarvestHelper.Equipment.Service.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace HarvestHelper.Equipment.Tests;

public class EquipmentControllerInteTests : IClassFixture<WebApplicationFactory<Startup>>
{
	private readonly WebApplicationFactory<Startup> factory;

	public EquipmentControllerInteTests(WebApplicationFactory<Startup> factory)
	{
		this.factory = factory;
	}

	[Fact]
	public async Task GetAsync_ReturnsEquipmentItems()
	{
		// Arrange
		var client = factory.CreateClient();
		var dbContext = factory.Services.GetRequiredService<DbContext>();
		await dbContext.Database.EnsureDeletedAsync();
		await dbContext.Database.EnsureCreatedAsync();

		var equipmentItems = new List<EquipmentItem>
		{
			new EquipmentItem { Name = "Item 1" },
			new EquipmentItem { Name = "Item 2" },
			new EquipmentItem { Name = "Item 3" },
		};
		await dbContext.AddRangeAsync(equipmentItems);
		await dbContext.SaveChangesAsync();

		// Act
		var response = await client.GetAsync("/equipment");
		response.EnsureSuccessStatusCode();
		var responseContent = await response.Content.ReadAsStringAsync();
		var items = JsonConvert.DeserializeObject<IEnumerable<EquipmentDto>>(responseContent);

		// Assert
		Assert.Equal(equipmentItems.Count, items.Count());
		foreach (var item in equipmentItems)
		{
			Assert.Contains(items, i => i.Id == item.Id && i.Name == item.Name);
		}
	}
}*/



/*using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HarvestHelper.Common;
using HarvestHelper.Equipment.Contracts;
using HarvestHelper.Equipment.Service;
using HarvestHelper.Equipment.Service.Controllers;
using HarvestHelper.Equipment.Service.Dtos;
using HarvestHelper.Equipment.Service.Entities;
using HarvestHelper.Inventory.Service;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using WebMotions.Fake.Authentication.JwtBearer;
using Xunit;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using System.Net.Http.Headers;

namespace HarvestHelper.Equipment.Tests;

public class EquipmentIntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
{
	private HttpClient _httpClient;
	private WebApplicationFactory<Startup> webApplicationFactory;

	public EquipmentIntegrationTest(WebApplicationFactory<Startup> factory)
	{
		var webApplicationFactory = factory;
		_httpClient = webApplicationFactory.CreateDefaultClient();
	}

	*//*[Fact]
	public async Task GetAsync_ReturnsEquipmentItems()
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eejtd....");
		_httpClient.DefaultRequestHeaders.Accept.Clear();
		_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
		_httpClient.DefaultRequestHeaders.Host = "harvesthelper.westeurope.cloudapp.azure.com";

		var response = await _httpClient.GetAsync("/equipment");
		var result = await response.Content.ReadAsStringAsync();
		Console.Write(result);
		Assert.True(!string.IsNullOrEmpty(result));
	}*/
/*
	[Fact]
	public async Task test()
	{
		var builder = new WebHostBuilder().UseStartup<TestStartup>();
		var testServer = new TestServer(builder);
		var client = testServer.CreateClient();

		var result = await client.GetAsync("/equipment");

		Assert.Equal(HttpStatusCode.OK, result.StatusCode);
	}*/
/*[Fact]
public async Task GetAsync_ReturnsEquipmentItems()
{
	// Arrange
	var client = webApplicationFactory.CreateClient();
	var equipmentRepository = webApplicationFactory.Services.GetService<IRepository<EquipmentItem>>();

	using var scope = webApplicationFactory.Services.CreateScope();
	var publishEndpoint = scope.ServiceProvider.GetService<IPublishEndpoint>();

	var equipment1 = new EquipmentItem { Name = "Equipment 1" };
	var equipment2 = new EquipmentItem { Name = "Equipment 2" };
	await equipmentRepository.CreateAsync(equipment1);
	await equipmentRepository.CreateAsync(equipment2);

	// Set the authorization header
	client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eejtd....");
	client.DefaultRequestHeaders.Accept.Clear();
	client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
	client.DefaultRequestHeaders.Host = "harvesthelper.westeurope.cloudapp.azure.com";

	// Act
	var response = await client.GetAsync("/equipment");
	var responseContent = await response.Content.ReadAsStringAsync();
	var items = JsonConvert.DeserializeObject<IEnumerable<EquipmentDto>>(responseContent);

	// Assert
	Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	Assert.Equal(2, items.Count());
	Assert.Contains(items, item => item.Id == equipment1.Id && item.Name == equipment1.Name);
	Assert.Contains(items, item => item.Id == equipment2.Id && item.Name == equipment2.Name);
}*//*
}
*/