using Domain.Enums;

namespace Application.Characters.Models;

public class CharacterDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Species { get; set; }

    public Gender Gender { get; set; }

    public LocationDto? Location { get; set; }
}