using Domain.Common;

namespace Domain.Entities;

public class Location : BaseEntity
{
    public required string Name { get; set; }

    public required string Type { get; set; }
    
    public string? Dimension { get; set; }

    public List<Character> Characters { get; set; } = new();
}