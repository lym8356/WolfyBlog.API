using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WolfyBlog.API.Database;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;
using WolfyBlog.API.Services.Interfaces;

namespace WolfyBlog.API.Services
{
	public class SiteLogRepository : ISiteLogRepository
	{
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SiteLogRepository(DataContext context, IMapper mapper)
		{
            _mapper = mapper;
            _context = context;
        }

        public SiteLog CreateSiteLog(SiteLogForCreationDTO siteLogForCreationDTO)
        {
            var siteLog = _mapper.Map<SiteLog>(siteLogForCreationDTO);
            _context.SiteLogs.Add(siteLog);
            return siteLog;
        }

        public void DeleteSiteLog(SiteLog siteLog)
        {
            _context.SiteLogs.Remove(siteLog);
        }

        public SiteLog EditSiteLog(SiteLog siteLogFromRepo, SiteLogForUpdateDTO siteLogForUpdateDTO)
        {
            _mapper.Map(siteLogForUpdateDTO, siteLogFromRepo);
            _context.Update(siteLogFromRepo);
            return siteLogFromRepo;
        }

        public async Task<SiteLog> GetSiteLogAsync(int id)
        {
            return await _context.SiteLogs.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<SiteLog>> GetSiteLogsAsync()
        {
            return await _context.SiteLogs.ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<bool> SiteLogExistAsync(int id)
        {
            return await _context.SiteLogs.AnyAsync(s => s.Id == id);
        }
    }
}

