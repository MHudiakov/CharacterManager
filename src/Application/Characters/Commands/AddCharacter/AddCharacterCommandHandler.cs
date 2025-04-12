using Application.Characters.Models;
using Application.Common.Exceptions;
using Application.Contracts;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Characters.Commands.AddCharacter;

public class AddCharacterCommandHandler : IRequestHandler<AddCharacterCommand, CharacterDto>
{
    private readonly IApplicationDbContext _context;

    private readonly ICacheService _cacheService;

    private readonly IMapper _mapper;

    public AddCharacterCommandHandler(IApplicationDbContext context, IMapper mapper, ICacheService cacheService)
    {
        _context = context;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<CharacterDto> Handle(AddCharacterCommand request, CancellationToken cancellationToken)
    {
        var location = await _context.Locations
            .FirstOrDefaultAsync(l => l.Id == request.LocationId, cancellationToken);

        if (location == null)
        {
            throw new NotFoundException("Location not found.");
        }

        var character = new Character
        {
            Name = request.Name,
            Species = request.Species,
            Gender = request.Gender,
            LocationId = request.LocationId
        };

        _context.Characters.Add(character);

        await _context.SaveChangesAsync(cancellationToken);

        await _cacheService.ClearCacheAsync();

        var characterDto = _mapper.Map<CharacterDto>(character);

        return characterDto;
    }
}