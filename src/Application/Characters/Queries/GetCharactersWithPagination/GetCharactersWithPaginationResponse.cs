using Application.Common.Models;

namespace Application.Characters.Queries.GetCharactersWithPagination;

public record GetCharactersWithPaginationResponse
{
    public PaginatedList<CharacterDto> Characters { get; init; }

    public bool IsFetchedFromDatabase { get; init; }
}