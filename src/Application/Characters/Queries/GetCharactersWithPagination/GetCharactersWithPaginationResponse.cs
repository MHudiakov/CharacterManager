using Application.Characters.Models;
using Application.Common.Models;

namespace Application.Characters.Queries.GetCharactersWithPagination;

public record GetCharactersWithPaginationResponse
{
    public required PaginatedList<CharacterDto> Characters { get; init; }

    public bool IsFetchedFromDatabase { get; init; }
}