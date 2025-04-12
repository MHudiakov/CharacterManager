using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Characters.Queries.GetCharactersWithPagination;

public record GetCharactersWithPaginationQuery : IRequest<GetCharactersWithPaginationResponse>
{
    [Range(1, int.MaxValue, ErrorMessage = "Page size must be greater than 0.")]
    public int PageNumber { get; init; } = 1;

    [Range(1, int.MaxValue, ErrorMessage = "Page size must be greater than 0.")]
    public int PageSize { get; set; } = 20;
}