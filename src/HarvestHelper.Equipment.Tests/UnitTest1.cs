using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestHelper.Common;
using HarvestHelper.Equipment.Service.Controllers;
using HarvestHelper.Equipment.Service.Dtos;
using HarvestHelper.Equipment.Service.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HarvestHelper.Equipment.Tests;

public class UnitTest1
{
	private readonly Mock<IRepository<EquipmentItem>> equipmentRepositoryMock;
	private readonly Mock<IPublishEndpoint> publishEndpointMock;
	private readonly EquipmentController equipmentController;

	public UnitTest1()
	{
		equipmentRepositoryMock = new Mock<IRepository<EquipmentItem>>();
		publishEndpointMock = new Mock<IPublishEndpoint>();
		equipmentController = new EquipmentController(equipmentRepositoryMock.Object, publishEndpointMock.Object);
	}

	[Fact]
	public async Task GetAsync_WithValidData_ReturnsOkResultWithEquipmentList()
	{
		// Arrange
		var equipmentItems = new List<EquipmentItem>
			{
				new EquipmentItem { Id = Guid.NewGuid(), Name = "Equipment 1" },
				new EquipmentItem { Id = Guid.NewGuid(), Name = "Equipment 2" }
			};

		equipmentRepositoryMock.Setup(repo => repo.GetAllAsync())
			.ReturnsAsync(equipmentItems);

		// Act
		var result = await equipmentController.GetAsync();

		// Assert
		Assert.IsType<OkObjectResult>(result.Result);

		var okResult = result.Result as OkObjectResult;
		var items = okResult.Value as IEnumerable<EquipmentDto>;

		Assert.Equal(equipmentItems.Count, items.Count());
		Assert.Equal(equipmentItems[0].Id, items.First().Id);
		Assert.Equal(equipmentItems[1].Name, items.Last().Name);
	}

	[Fact]
	public async Task GetByIdAsync_WithValidId_ReturnsOkResultWithEquipment()
	{
		// Arrange
		var equipmentId = Guid.NewGuid();
		var equipmentItem = new EquipmentItem { Id = equipmentId, Name = "Equipment" };

		equipmentRepositoryMock.Setup(repo => repo.GetAsync(equipmentId))
			.ReturnsAsync(equipmentItem);

		// Act
		var result = await equipmentController.GetByIdAsync(equipmentId);
		Console.WriteLine(result.Result);

		// Assert
		Assert.IsType<OkObjectResult>(result.Result);

		var okResult = result.Result as OkObjectResult;
		var item = okResult.Value as EquipmentDto;

		Assert.Equal(equipmentItem.Id, item.Id);
		Assert.Equal(equipmentItem.Name, item.Name);
	}
}