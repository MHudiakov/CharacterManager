using Application.Common.Models;

namespace Application.Locations.Queries.GetCharactersByLocation;

public class GetCharactersByLocationResponse
{
    public required PaginatedList<CharacterByLocationDto> Characters { get; set; }

    public bool IsFetchedFromDatabase { get; init; }
}