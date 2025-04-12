using Application.Common.Models;
using Application.Locations.Queries.GetCharactersByLocation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class PlanetController : ApiController
{
    [HttpGet("characters")]
    [ProducesResponseType(typeof(PaginatedList<CharacterByLocationDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetCharactersByLocation(
        [FromQuery] GetCharactersByLocationQuery query,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await Mediator.Send(query, cancellationToken);

        Response.Headers.Append("from-database", response.IsFetchedFromDatabase.ToString());

        return Ok(response.Characters);
    }
}