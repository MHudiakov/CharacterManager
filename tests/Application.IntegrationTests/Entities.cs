using Domain.Entities;
using Domain.Enums;

namespace Application.IntegrationTests;

public static class Entities
{
    public static List<Character> GetCharacters(int locationId)
    {
        return
        [
            new()
            {
                Id = 1,
                Name = "Tim",
                Species = "Human",
                Gender = Gender.Male,
                LocationId = locationId,
            },
            new()
            {
                Id = 2,
                Name = "Ben",
                Species = "Human",
                Gender = Gender.Male,
                LocationId = locationId
            }
        ];
    }

    public static List<Location> GetLocations()
    {
        return
        [
            new()
            {
                Id = 1,
                Name = "Earth",
                Type = "Earth type"
            }
        ];
    }
}