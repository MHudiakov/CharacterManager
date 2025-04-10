using Domain.Enums;

namespace Application.Locations.Queries.GetCharactersByLocation;

public class CharacterByLocationDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Species { get; set; }

    public Gender Gender { get; set; }
}