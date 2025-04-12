namespace Application.Characters.Models;

public class LocationDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Type { get; set; }

    public string? Dimension { get; set; }
}