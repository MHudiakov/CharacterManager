using Domain.Enums;
using FluentAssertions;

namespace Application.IntegrationTests.Characters.Commands;

using Application.Characters.Commands.AddCharacter;
using Common.Exceptions;
using static Testing;

public class AddCharacterTests : BaseTestFixture
{
    [Test]
    public async Task ShouldCreateCharacter_WhenLocationExists()
    {
        // Arrange
        var locations = Entities.GetLocations();
        await AddAsync(locations);

        var command = new AddCharacterCommand
        {
            Name = "Rick Sanchez",
            Species = "Human",
            Gender = Gender.Male,
            LocationId = locations.First().Id
        };

        // Act
        var result = await SendAsync(command);

        // Assert
        result.Name.Should().Be(command.Name);
        result.Species.Should().Be(command.Species);
        result.Gender.Should().Be(command.Gender);
        result.Location!.Id.Should().Be(locations.First().Id);
    }

    [Test]
    public void ShouldThrowNotFoundException_WhenLocationDoesNotExist()
    {
        // Arrange
        var command = new AddCharacterCommand
        {
            Name = "Rick Sanchez",
            Species = "Human",
            Gender = Gender.Male,
            LocationId = 999
        };

        // Act & Assert
        var exception = Assert.ThrowsAsync<NotFoundException>(
            () => SendAsync(command));

        exception.Message.Should().Contain("Location not found.");
    }
}