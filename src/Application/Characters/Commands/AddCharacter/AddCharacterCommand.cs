using Application.Characters.Models;
using Domain.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Characters.Commands.AddCharacter;

public class AddCharacterCommand : IRequest<CharacterDto>
{
    public required string Name { get; init; }

    public required string Species { get; init; }
    
    public Gender Gender { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "Location Id must be greater than 0.")]
    public int LocationId { get; init; }
}