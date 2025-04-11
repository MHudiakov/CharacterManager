using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Locations.Queries.GetCharactersByLocation;

public class GetCharactersByLocationQuery : IRequest<GetCharactersByLocationResponse>
{
    [Required]
    public required string Location { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "Page size must be greater than 0.")]
    public int PageNumber { get; set; } = 1;

    [Range(1, int.MaxValue, ErrorMessage = "Page size must be greater than 0.")]
    public int PageSize { get; set; } = 20;
}