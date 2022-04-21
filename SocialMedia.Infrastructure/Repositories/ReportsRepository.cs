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
            if (!_appDbContext.Reports.Any(e=> e.PostId == r.PostId))
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
            return await Task.FromResult(_appDbContext.Reports.Include(p => p.Reporters).Include(p => p.Post));
        }

        public async Task DelAsync(int id)
        {
            try
            {
                _appDbContext.Reports.Remove(_appDbContext.Reports.Include(p => p.Reporters).Include(p => p.Post).FirstOrDefault(x => x.Id == id));
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
                _appDbContext.Reports.Remove(_appDbContext.Reports.Include(p => p.Post).FirstOrDefault(x => x.Post.Id == postId));
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
            return await Task.FromResult(_appDbContext.Reports.Include(p => p.Post).Include(p => p.Reporters).FirstOrDefault(x => x.Id == id));
        }

        public async Task<Reports> GetPostReports(int postId)
        {
            return await Task.FromResult(_appDbContext.Reports.Include(p => p.Post).Include(p => p.Reporters).FirstOrDefault(x => x.Post.Id == postId));
        }

        public async Task ReportPost(int postId, UserData userData)
        {
            try
            {
                var z = _appDbContext.Reports.Include(p => p.Reporters).FirstOrDefault(x => x.PostId == postId);

                if (z.Reporters != null)
                {
                    if (z.Reporters.Exists(z => z.Id == userData.Id))
                    {
                        z.Reporters.Remove(z.Reporters.Find(z => z.Id == userData.Id));
                    }
                    else
                    {
                        z.Reporters.Add(userData);
                    }
                }
                else
                {
                    z.Reporters = new List<UserData>
                    {
                        userData
                    };
                }

                await _appDbContext.SaveChangesAsync();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }
    }
}
