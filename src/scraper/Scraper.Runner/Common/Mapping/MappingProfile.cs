using AutoMapper;
using Scraper.ApiClient.Models;
using Domain.Entities;

namespace Scraper.Runner.Common.Mapping;

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CharacterDto, Character>()
            .ForMember(dest => dest.Location, opt => opt.Ignore());

        CreateMap<LocationDto, Location>();
    }
}