using AutoMapper;
using HNBestStories.Models;
using HNBestStories.Utils;
using System;

namespace HNBestStories.Mappers
{
    public class StoryAMProfile : Profile
    {
        public StoryAMProfile()
        {
            CreateMap<Story, StoryDTO>().ReverseMap();
            CreateMap<double, DateTime>().ConvertUsing(new DateTimeUnixConverter());
        }
    }
}
