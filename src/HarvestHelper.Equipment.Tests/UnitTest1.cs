using System;
using HarvestHelper.Equipment.Service;
using HarvestHelper.Equipment.Service.Entities;

namespace HarvestHelper.Equipment.Tests;

public class UnitTest1
{
    [Fact]
    public void AsDto_ConvertsEquipmentItemToEquipmentDto()
    {
        // Arrange
        var equipmentItem = new EquipmentItem
        {
            Id = Guid.NewGuid(),
            Name = "Excavator",
            DateAdded = new DateTime(2023, 1, 1)
        };

        // Act
        var result = equipmentItem.AsDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(equipmentItem.Id, result.Id);
        Assert.Equal(equipmentItem.Name, result.Name);
        Assert.Equal(equipmentItem.DateAdded, result.DateAdded);
    }
}