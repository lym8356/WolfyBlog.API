using System;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Services.Interfaces
{
	public interface IAboutPageRepository
	{
        Task<AboutPage> GetAboutPageAsync(int aboutPageId);
        Task<IEnumerable<AboutPage>> GetAboutPagesAsync();
        Task<AboutPage> GetNotificationAsync();
        Task<IEnumerable<AboutPage>> CheckDuplicate(bool isAboutSite, bool isNotification);
        AboutPage CreateAboutPage(AboutPage aboutPage);
        //AboutPage EditAboutPage(AboutPage aboutFromRepo, AboutPage aboutUpdated);
        Task<bool> AboutPageExistAsync(int aboutPageId);
        Task<bool> NotificationExistAsync();
        Task<bool> SaveAsync();
    }
}

