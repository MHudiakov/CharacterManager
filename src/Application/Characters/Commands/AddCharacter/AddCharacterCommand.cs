using Application.Characters.Models;
using Domain.Enums;
using MediatR;

namespace Application.Characters.Commands.AddCharacter;

public class AddCharacterCommand : IRequest<CharacterDto>
{
    public string Name { get; init; }

    public string Species { get; init; }
    
    public Gender Gender { get; init; }
    
    public int LocationId { get; init; }
}