using System;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Services.Interfaces
{
	public interface IAboutPageRepository
	{
        Task<AboutPage> GetAboutPageAsync(int aboutPageId);
        Task<IEnumerable<AboutPage>> GetAboutPagesAsync();
        Task<IEnumerable<AboutPage>> CheckDuplicate(bool isAboutSite);
        AboutPage CreateAboutPage(AboutPage aboutPage);
        //AboutPage EditAboutPage(AboutPage aboutFromRepo, AboutPage aboutUpdated);
        Task<bool> AboutPageExistAsync(int aboutPageId);
        Task<bool> SaveAsync();
    }
}

