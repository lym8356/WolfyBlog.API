using System;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Services.Interfaces
{
	public interface ISiteLogRepository
	{
        Task<bool> SiteLogExistAsync(int id);
        Task<IEnumerable<SiteLog>> GetSiteLogsAsync();
        Task<SiteLog> GetSiteLogAsync(int id);
        SiteLog CreateSiteLog(SiteLogForCreationDTO siteLogForCreationDTO);
        SiteLog EditSiteLog(SiteLog siteLogFromRepo, SiteLogForUpdateDTO siteLogForUpdateDTO);
        void DeleteSiteLog(SiteLog siteLog);
        Task<bool> SaveAsync();
    }
}

