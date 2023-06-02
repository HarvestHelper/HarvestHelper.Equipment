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

// public class ControlllerTest
// {
//     private readonly EquipmentController _controller;
//     private readonly Mock<IRepository<EquipmentItem>> _mockRepository;
//     private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;

//     public ControlllerTest()
//     {
//         _mockRepository = new Mock<IRepository<EquipmentItem>>();
//         _mockPublishEndpoint = new Mock<IPublishEndpoint>();

//         _controller = new EquipmentController(_mockRepository.Object, _mockPublishEndpoint.Object);
//     }

//     [Fact]
//     public async Task GetAsync_ReturnsOkResultWithEquipmentDtos()
//     {
//         // Arrange
//         var equipmentItems = new List<EquipmentItem>
//         {
//             new EquipmentItem { Id = Guid.NewGuid(), Name = "Excavator", DateAdded = DateTime.Now },
//             new EquipmentItem { Id = Guid.NewGuid(), Name = "Bulldozer", DateAdded = DateTime.Now }
//         };

//         _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(equipmentItems);

//         // Act
//         var result = await _controller.GetAsync();

//         // Assert
//         var okResult = Assert.IsType<OkObjectResult>(result.Result);
//         var items = Assert.IsAssignableFrom<IEnumerable<EquipmentDto>>(okResult.Value);
//         Assert.Equal(2, items.Count());
//         Assert.Equal(equipmentItems[0].Id, items.First().Id);
//         Assert.Equal(equipmentItems[0].Name, items.First().Name);
//         Assert.Equal(equipmentItems[0].DateAdded, items.First().DateAdded);
//         Assert.Equal(equipmentItems[1].Id, items.Last().Id);
//         Assert.Equal(equipmentItems[1].Name, items.Last().Name);
//         Assert.Equal(equipmentItems[1].DateAdded, items.Last().DateAdded);
//     }

//     [Fact]
//     public async Task GetByIdAsync_WithValidId_ReturnsOkResultWithEquipmentDto()
//     {
//         // Arrange
//         var itemId = Guid.NewGuid();
//         var equipmentItem = new EquipmentItem
//         {
//             Id = itemId,
//             Name = "Excavator",
//             DateAdded = DateTime.Now
//         };

//         _mockRepository.Setup(repo => repo.GetAsync(itemId)).ReturnsAsync(equipmentItem);

//         // Act
//         var result = await _controller.GetByIdAsync(itemId);

//         // Assert
//         var okResult = Assert.IsType<OkObjectResult>(result.Result);
//         var item = Assert.IsType<EquipmentDto>(okResult.Value);
//         Assert.Equal(equipmentItem.Id, item.Id);
//         Assert.Equal(equipmentItem.Name, item.Name);
//         Assert.Equal(equipmentItem.DateAdded, item.DateAdded);
//     }

//     [Fact]
//     public async Task GetByIdAsync_WithInvalidId_ReturnsNotFoundResult()
//     {
//         // Arrange
//         var itemId = Guid.NewGuid();

//         _mockRepository.Setup(repo => repo.GetAsync(itemId)).ReturnsAsync((EquipmentItem)null);

//         // Act
//         var result = await _controller.GetByIdAsync(itemId);

//         // Assert
//         Assert.IsType<NotFoundResult>(result.Result);
//     }


// 	[Fact]
// 	public async Task PutAsync_WithValidIdAndDto_ReturnsNoContentResult()
// 	{
// 		// Arrange
// 		var itemId = Guid.NewGuid();
// 		var updateDto = new UpdateEquipmentDto ("Bulldozer");
// 		var existingEquipment = new EquipmentItem { Id = itemId };

// 		_mockRepository.Setup(repo => repo.GetAsync(itemId)).ReturnsAsync(existingEquipment);
// 		_mockRepository.Setup(repo => repo.UpdateAsync(existingEquipment)).Returns(Task.CompletedTask);
// 		_mockPublishEndpoint.Setup(endpoint => endpoint.Publish(It.IsAny<EquipmentItemUpdated>(), default)).Returns(Task.CompletedTask);

// 		// Act
// 		var result = await _controller.PutAsync(itemId, updateDto);

// 		// Assert
// 		Assert.IsType<NoContentResult>(result);
// 	}

// 	[Fact]
// 	public async Task PutAsync_WithInvalidId_ReturnsNotFoundResult()
// 	{
// 		// Arrange
// 		var itemId = Guid.NewGuid();
// 		var updateDto = new UpdateEquipmentDto ("Bulldozer");

// 		_mockRepository.Setup(repo => repo.GetAsync(itemId)).ReturnsAsync((EquipmentItem)null);

// 		// Act
// 		var result = await _controller.PutAsync(itemId, updateDto);

// 		// Assert
// 		Assert.IsType<NotFoundResult>(result);
// 	}

// 	[Fact]
// 	public async Task DeleteAsync_WithValidId_ReturnsNoContentResult()
// 	{
// 		// Arrange
// 		var itemId = Guid.NewGuid();
// 		var existingEquipment = new EquipmentItem { Id = itemId };

// 		_mockRepository.Setup(repo => repo.GetAsync(itemId)).ReturnsAsync(existingEquipment);
// 		_mockRepository.Setup(repo => repo.RemoveAsync(existingEquipment.Id)).Returns(Task.CompletedTask);
// 		_mockPublishEndpoint.Setup(endpoint => endpoint.Publish(It.IsAny<EquipmentItemDeleted>(), default)).Returns(Task.CompletedTask);

// 		// Act
// 		var result = await _controller.DeleteAsync(itemId);

// 		// Assert
// 		Assert.IsType<NoContentResult>(result);
// 	}

// 	[Fact]
// 	public async Task DeleteAsync_WithInvalidId_ReturnsNotFoundResult()
// 	{
// 		// Arrange
// 		var itemId = Guid.NewGuid();

// 		_mockRepository.Setup(repo => repo.GetAsync(itemId)).ReturnsAsync((EquipmentItem)null);

// 		// Act
// 		var result = await _controller.DeleteAsync(itemId);

// 		// Assert
// 		Assert.IsType<NotFoundResult>(result);
// 	}


// }

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