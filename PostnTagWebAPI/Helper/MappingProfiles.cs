using AutoMapper;
using PostnTagWebAPI.Dto;
using PostnTagWebAPI.Models;

namespace PostnTagWebAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();
            CreateMap<Tag, TagDto>();
            CreateMap<TagDto, Tag>();
            CreateMap<Post, PostDtoCreate>();
            CreateMap<PostDtoCreate, Post>();
            CreateMap<Tag, TagDtoCreate>();
            CreateMap<TagDtoCreate, Tag>();
        }        
    }
}
