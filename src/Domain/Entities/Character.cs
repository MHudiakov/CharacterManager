using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Character : BaseEntity
{
    public required string Name { get; set; }

    public Gender Gender { get; set; }
    
    public required string Species { get; set; }

    public int? LocationId { get; set; }

    public Location? Location { get; set; }
}