using Application.Characters.Models;
using Application.Common;
using Application.Common.Models;
using Application.Contracts;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Characters.Queries.GetCharactersWithPagination;

public class GetCharactersWithPaginationQueryHandler : IRequestHandler<GetCharactersWithPaginationQuery, GetCharactersWithPaginationResponse>
{
    private readonly IApplicationDbContext _context;

    private readonly ICacheService _cacheService;

    private readonly IMapper _mapper;

    private const int CacheDurationMinutes = 5;

    public GetCharactersWithPaginationQueryHandler(IApplicationDbContext context, ICacheService cacheService, IMapper mapper)
    {
        _context = context;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<GetCharactersWithPaginationResponse> Handle(GetCharactersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{CacheKeys.Characters}_{request.PageNumber}_{request.PageSize}";

        var cachedData = await _cacheService.GetCachedDataAsync<PaginatedList<CharacterDto>>(cacheKey);

        if (cachedData != null)
        {
            return new GetCharactersWithPaginationResponse
            {
                Characters = cachedData,
                IsFetchedFromDatabase = false
            };
        }

        var charactersQuery = _context.Characters
            .Include(c => c.Location)
            .AsNoTracking();

        var totalCount = await charactersQuery.CountAsync(cancellationToken);

        var characters = await charactersQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var characterDtos = _mapper.Map<List<CharacterDto>>(characters);

        var paginatedList = new PaginatedList<CharacterDto>(characterDtos, totalCount, request.PageNumber, request.PageSize);

        await _cacheService.SetCacheAsync(cacheKey, paginatedList, TimeSpan.FromMinutes(CacheDurationMinutes));

        return new GetCharactersWithPaginationResponse
        {
            Characters = paginatedList,
            IsFetchedFromDatabase = true
        };
    }
}