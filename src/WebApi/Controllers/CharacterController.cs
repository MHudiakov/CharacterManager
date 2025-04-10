using Application.Characters.Queries.GetCharactersWithPagination;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class CharacterController : ApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(GetCharactersWithPaginationResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetCharacters(
        [FromQuery] GetCharactersWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var response = await Mediator.Send(query, cancellationToken);

        Response.Headers.Append("X-Fetched-From-Database", response.IsFetchedFromDatabase.ToString());

        return Ok(response.Characters);
    }
}