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
                Name = "Tim",
                Species = "Human",
                Gender = Gender.Male,
                LocationId = locationId,
            },
            new()
            {
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
                Name = "Earth",
                Type = "Earth type"
            }
        ];
    }
}