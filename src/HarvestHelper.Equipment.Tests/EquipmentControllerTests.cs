using System;
using HarvestHelper.Common;
using HarvestHelper.Equipment.Service.Controllers;
using HarvestHelper.Equipment.Service.Dtos;
using HarvestHelper.Equipment.Service.Entities;
using MassTransit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using HarvestHelper.Equipment.Contracts;

namespace HarvestHelper.Equipment.Tests;

public class EquipmentControllerTests
{
    private readonly EquipmentController controller;
    private readonly Mock<IRepository<EquipmentItem>> equipmentRepositoryMock;
    private readonly Mock<IPublishEndpoint> publishEndpointMock;

    public EquipmentControllerTests()
    {
        equipmentRepositoryMock = new Mock<IRepository<EquipmentItem>>();
        publishEndpointMock = new Mock<IPublishEndpoint>();
        controller = new EquipmentController(equipmentRepositoryMock.Object, publishEndpointMock.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsOkResult_WithListOfEquipmentItems()
    {
        // Arrange
        var equipmentItems = new List<EquipmentItem>
        {
            new EquipmentItem { Id = Guid.NewGuid(), Name = "Equipment 1" },
            new EquipmentItem { Id = Guid.NewGuid(), Name = "Equipment 2" },
        };
        equipmentRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(equipmentItems);

        // Act
        var result = await controller.GetAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var items = Assert.IsAssignableFrom<IEnumerable<EquipmentDto>>(okResult.Value);
        Assert.Equal(equipmentItems.Count, items.Count());
        Assert.Equal(equipmentItems[0].Name, items.ElementAt(0).Name);
        Assert.Equal(equipmentItems[1].Name, items.ElementAt(1).Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNotFound_WhenEquipmentItemNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        equipmentRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync((EquipmentItem)null);

        // Act
        var result = await controller.GetByIdAsync(id);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOkResult_WithEquipmentItem()
    {
        // Arrange
        var id = Guid.NewGuid();
        var equipmentItem = new EquipmentItem { Id = id, Name = "Equipment 1" };
        equipmentRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(equipmentItem);

        // Act
        var result = await controller.GetByIdAsync(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var item = Assert.IsType<EquipmentDto>(okResult.Value);
        Assert.Equal(equipmentItem.Name, item.Name);
    }

    [Fact]
    public async Task PutAsync_ReturnsNotFound_WhenEquipmentItemNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateEquipmentDto = new UpdateEquipmentDto("Updated Equipment");
        equipmentRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync((EquipmentItem)null);

        // Act
        var result = await controller.PutAsync(id, updateEquipmentDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task PutAsync_ReturnsNoContentResult_WhenEquipmentItemUpdated()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingEquipment = new EquipmentItem { Id = id, Name = "Equipment 1" };
        var updateEquipmentDto = new UpdateEquipmentDto("Updated Equipment");
        equipmentRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(existingEquipment);

        // Act
        var result = await controller.PutAsync(id, updateEquipmentDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(updateEquipmentDto.Name, existingEquipment.Name);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotFound_WhenEquipmentItemNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        equipmentRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync((EquipmentItem)null);

        // Act
        var result = await controller.DeleteAsync(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContentResult_WhenEquipmentItemDeleted()
    {
        // Arrange
        var id = Guid.NewGuid();
        var equipment = new EquipmentItem { Id = id, Name = "Equipment 1" };
        equipmentRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(equipment);

        // Act
        var result = await controller.DeleteAsync(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        equipmentRepositoryMock.Verify(x => x.RemoveAsync(id), Times.Once);
    }

    [Fact]
    public async Task PostAsync_ReturnsCreatedAtActionResult_WithEquipmentItem()
    {
        // Arrange
        var createEquipmentDto = new CreateEquipmentDto("New Equipment");
        var equipment = new EquipmentItem { Id = Guid.NewGuid(), Name = createEquipmentDto.Name };
        equipmentRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<EquipmentItem>())).Returns(Task.CompletedTask);

        // Act
        var result = await controller.PostAsync(createEquipmentDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var item = Assert.IsType<EquipmentItem>(createdAtActionResult.Value);
        Assert.Equal(equipment.Name, item.Name);
    }
}