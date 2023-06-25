using System;
using System.IO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WolfyBlog.API.Database;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;
using WolfyBlog.API.Services.Interfaces;

namespace WolfyBlog.API.Services
{
	public class AboutPageRepository : IAboutPageRepository
	{
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AboutPageRepository(DataContext context, IMapper mapper)
		{
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> AboutPageExistAsync(int aboutPageId)
        {
            return await _context.AboutPage.AnyAsync(a => a.Id == aboutPageId);
        }

        public async Task<bool> NotificationExistAsync()
        {
            return await _context.AboutPage.AnyAsync(a => a.IsNotification == true);
        }

       
        public AboutPage CreateAboutPage(AboutPage aboutPage)
        {
            
            _context.AboutPage.Add(aboutPage);
            return aboutPage;
        }

        //public AboutPage EditAboutPage(AboutPage aboutPageFromRepo, AboutPage newAboutPage)
        //{
        //    aboutPageFromRepo = newAboutPage;
        //    _context.Update(aboutPageFromRepo);
        //    return aboutPageFromRepo;
        //}

        public async Task<AboutPage> GetAboutPageAsync(int aboutPageId)
        {
            return await _context.AboutPage.FirstOrDefaultAsync(a => a.Id == aboutPageId);  
        }

        public async Task<AboutPage> GetNotificationAsync()
        {
            return await _context.AboutPage.FirstOrDefaultAsync(a => a.IsNotification == true);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<IEnumerable<AboutPage>> CheckDuplicate(bool isAboutSite, bool isNotification)
        {
            return await _context.AboutPage.Where(a => a.IsAboutSite == isAboutSite)
                .Where(a => a.IsNotification == isNotification)
                .ToListAsync();
        }

        public async Task<IEnumerable<AboutPage>> GetAboutPagesAsync()
        {
            return await _context.AboutPage.ToListAsync();
        }
    }
}

