using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Domain;
using SocialMedia.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class VotesRepository : IVotesRepository
    {
        private readonly AppDbContext _appDbContext;

        public VotesRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Votes v)
        {
            if (!_appDbContext.Votes.Any(e=> e.PostId == v.PostId))
            {
                try
                {
                    _appDbContext.Votes.Add(v);
                    _appDbContext.SaveChanges();
                    await Task.CompletedTask;
                }
                catch (Exception ex)
                {
                    await Task.FromException(ex);
                }
            }
        }

        public async Task<IEnumerable<Votes>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.Votes.Include(p => p.Upvoters).Include(p => p.Post));
        }

        public async Task DelAsync(int id)
        {
            try
            {
                _appDbContext.Votes.Remove(_appDbContext.Votes.Include(p => p.Upvoters).Include(p => p.Post).FirstOrDefault(x => x.Id == id));
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task DelPostVotes(int postId)
        {
            try
            {
                _appDbContext.Votes.Remove(_appDbContext.Votes.Include(p => p.Post).FirstOrDefault(x => x.Post.Id == postId));
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task<Votes> GetAsyncById(int id)
        {
            return await Task.FromResult(_appDbContext.Votes.Include(p => p.Post).Include(p => p.Upvoters).FirstOrDefault(x => x.Id == id));
        }

        public async Task<Votes> GetPostVotes(int postId)
        {
            return await Task.FromResult(_appDbContext.Votes.Include(p => p.Post).Include(p => p.Upvoters).FirstOrDefault(x => x.Post.Id == postId));
        }

        public async Task UpvotePost(int postId, UserData userData)
        {
            try
            {
                var z = _appDbContext.Votes.Include(p => p.Post).Include(p => p.Upvoters).FirstOrDefault(x => x.PostId == postId);

                if (z.Upvoters != null && z.Upvoters.Count > 0)
                {
                    if (z.Upvoters.Exists(z => z.Id == userData.Id))
                    {
                        z.Upvoters.Remove(z.Upvoters.Find(z => z.Id == userData.Id));
                    }
                    else
                    {
                        z.Upvoters.Add(userData);
                    }
                }
                else
                {
                    z.Upvoters = new List<UserData>();
                    z.Upvoters.Add(userData);
                }

                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }
    }
}
