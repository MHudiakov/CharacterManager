using Domain.Enums;

namespace Application.Characters.Models;

public class CharacterDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Species { get; set; }

    public Gender Gender { get; set; }

    public LocationDto Location { get; set; }
}