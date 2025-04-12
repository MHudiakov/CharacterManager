using Application.Characters.Commands.AddCharacter;
using Application.Characters.Models;
using Application.Characters.Queries.GetCharactersWithPagination;
using Application.Common.Exceptions;
using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class GetCharactersWithPagination : ApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<CharacterDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetCharacters(
        [FromQuery] GetCharactersWithPaginationQuery query,
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

    [HttpPost]
    [ProducesResponseType(typeof(CharacterDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> AddCharacter(
        [FromBody] AddCharacterCommand command,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var characterDto = await Mediator.Send(command, cancellationToken);
            return StatusCode(201, characterDto);
        }
        catch (NotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}