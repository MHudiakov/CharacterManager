using Application.Common;
using Application.Common.Models;
using Application.Contracts;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Queries.GetCharactersByLocation;

public class GetCharactersByLocationQueryHandler : IRequestHandler<GetCharactersByLocationQuery, GetCharactersByLocationResponse>
{
    private readonly IApplicationDbContext _context;

    private readonly ICacheService _cacheService;
    
    private readonly IMapper _mapper;

    private const int CacheDurationMinutes = 5;

    public GetCharactersByLocationQueryHandler(IApplicationDbContext context, ICacheService cacheService, IMapper mapper)
    {
        _context = context;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<GetCharactersByLocationResponse> Handle(GetCharactersByLocationQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{CacheKeys.CacheByLocationPrefix}{request.Location.ToLower()}_{request.PageNumber}_{request.PageSize}";

        var cachedData = await _cacheService.GetCachedDataAsync<PaginatedList<CharacterByLocationDto>>(cacheKey);

        if (cachedData != null)
        {
            return new GetCharactersByLocationResponse
            {
                Characters = cachedData,
                IsFetchedFromDatabase = false
            };
        }

        var charactersQuery = _context.Characters
            .Include(c => c.Location)
            // No need to use .ToLower() since SQL Server is case-insensitive by default, besides, .ToLower() can bypass index scan
            .Where(c => c.Location != null && c.Location.Name == request.Location)
            .AsNoTracking();

        var totalCount = await charactersQuery.CountAsync(cancellationToken);

        var characters = await charactersQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var characterDtos = _mapper.Map<List<CharacterByLocationDto>>(characters);

        var paginatedList = new PaginatedList<CharacterByLocationDto>(characterDtos, totalCount, request.PageNumber, request.PageSize);

        await _cacheService.SetCacheAsync(cacheKey, paginatedList, TimeSpan.FromMinutes(CacheDurationMinutes));

        var result = new GetCharactersByLocationResponse
        {
            Characters = paginatedList,
            IsFetchedFromDatabase = true
        };

        return result;
    }
}