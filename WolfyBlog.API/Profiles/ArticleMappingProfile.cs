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
            CreateMap<Article, ArticleDTO>()
                .ForMember(d => d.Comments, o => o.MapFrom(src => src.Comments.OrderByDescending(c => c.CreatedAt)));
            //CreateMap<ArticleTag, Tag>()
            //    .ForMember(d => d.Title, o => o.MapFrom(s => s.Tag.Title));
            CreateMap<ArticleForCreationDTO, Article>();
            CreateMap<ArticleForUpdateDTO, Article>();
        }
    }
}

