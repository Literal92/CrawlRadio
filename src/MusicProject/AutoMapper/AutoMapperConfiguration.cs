using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MusicProject.Entities.Content;


namespace MusicProject.AutoMapper
{
    public class AutoMapperConfiguration : Profile
    {

        public AutoMapperConfiguration()
        {
            CreateMap<Content, ContentList>().ReverseMap();
            // CreateMap<Tag,ContentTag>().ReverseMap();
            // CreateMap<Category,CategoryTag>.ReverseMap();
        }
    }
}
