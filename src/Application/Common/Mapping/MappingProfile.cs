﻿using AutoMapper;
using Domain.Entities;
using Application.Characters.Models;
using Application.Locations.Queries.GetCharactersByLocation;

namespace Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Character, CharacterDto>();
        CreateMap<Character, CharacterByLocationDto>();
        CreateMap<Location, LocationDto>();
    }
}