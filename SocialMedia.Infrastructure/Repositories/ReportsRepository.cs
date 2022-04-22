using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Domain;
using SocialMedia.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly AppDbContext _appDbContext;

        public ReportsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Reports r)
        {
            if (!_appDbContext.Reports.Include(x => x.Post).Include(x => x.Reporter).Any(e=> e.Post.Id == r.Post.Id && e.Reporter.Id == r.Reporter.Id))
            {
                try
                {
                    _appDbContext.Reports.Add(r);
                    _appDbContext.SaveChanges();
                    await Task.CompletedTask;
                }
                catch (Exception ex)
                {
                    await Task.FromException(ex);
                }
            }
        }

        public async Task<IEnumerable<Reports>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.Reports.Include(p => p.Reporter).Include(p => p.Post));
        }

        public async Task DelAsync(int id)
        {
            try
            {
                _appDbContext.Reports.Remove(_appDbContext.Reports.FirstOrDefault(x => x.Id == id));
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task DelPostReports(int postId)
        {
            try
            {
                _appDbContext.Reports.RemoveRange(_appDbContext.Reports.Include(p => p.Post).Where(x => x.Post.Id == postId)) ;
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task<Reports> GetAsyncById(int id)
        {
            return await Task.FromResult(_appDbContext.Reports.Include(p => p.Post).Include(p => p.Reporter).FirstOrDefault(x => x.Id == id));
        }

        public async Task<Reports> GetPostReports(int postId)
        {
            return await Task.FromResult(_appDbContext.Reports.Include(p => p.Post).Include(p => p.Reporter).FirstOrDefault(x => x.Post.Id == postId));
        }
    }
}
