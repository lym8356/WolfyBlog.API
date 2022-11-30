using AutoMapper;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Profiles
{
    public class ArticleMappingProfile : Profile
    {
        public ArticleMappingProfile()
        {
            CreateMap<Article, Article>();
            CreateMap<Article, ArticleDTO>();
            //CreateMap<ArticleTag, Tag>()
            //    .ForMember(d => d.Title, o => o.MapFrom(s => s.Tag.Title));
            CreateMap<ArticleForCreationDTO, Article>();
        }
    }
}

