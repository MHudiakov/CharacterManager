using MediatR;

namespace Application.Characters.Queries.GetCharactersWithPagination;

public record GetCharactersWithPaginationQuery : IRequest<GetCharactersWithPaginationResponse>
{
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; set; } = 20;
}