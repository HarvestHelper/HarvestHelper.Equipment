using System;
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
    private readonly WebApplicationFactory<Startup> factory;

	public EquipmentIntegrationTest(WebApplicationFactory<Startup> factory)
    {
		this.factory = factory;
	}

	[Fact]
	public async Task GetAsync_ReturnsEquipmentItems()
	{
		// Arrange
		var client = factory.CreateClient();
		var equipmentRepository = factory.Services.GetService<IRepository<EquipmentItem>>();

		using var scope = factory.Services.CreateScope();
		var publishEndpoint = scope.ServiceProvider.GetService<IPublishEndpoint>();

		var equipment1 = new EquipmentItem { Name = "Equipment 1" };
		var equipment2 = new EquipmentItem { Name = "Equipment 2" };
		await equipmentRepository.CreateAsync(equipment1);
		await equipmentRepository.CreateAsync(equipment2);

		// Set the authorization header
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "your_access_token");

		// Act
		var response = await client.GetAsync("/equipment");
		var responseContent = await response.Content.ReadAsStringAsync();
		var items = JsonConvert.DeserializeObject<IEnumerable<EquipmentDto>>(responseContent);

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal(2, items.Count());
		Assert.Contains(items, item => item.Id == equipment1.Id && item.Name == equipment1.Name);
		Assert.Contains(items, item => item.Id == equipment2.Id && item.Name == equipment2.Name);
	}

	[Fact]
    public async Task PostAsync_CreatesEquipmentItem()
    {
        // Arrange
        var client = factory.CreateClient();
        var equipmentRepository = factory.Services.GetService<IRepository<EquipmentItem>>();
        var publishEndpoint = factory.Services.GetService<IPublishEndpoint>();

        var createEquipmentDto = new CreateEquipmentDto("New Equipment");
        var json = JsonConvert.SerializeObject(createEquipmentDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/equipment", content);
        var responseContent = await response.Content.ReadAsStringAsync();
        var item = JsonConvert.DeserializeObject<EquipmentDto>(responseContent);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(createEquipmentDto.Name, item.Name);

        var createdItem = await equipmentRepository.GetAsync(item.Id);
        Assert.NotNull(createdItem);
        Assert.Equal(createEquipmentDto.Name, createdItem.Name);
    }
}
