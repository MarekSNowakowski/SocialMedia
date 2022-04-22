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
            if (!_appDbContext.Votes.Include(x => x.Post).Include(x => x.Upvoter).Any(x => x.Upvoter.Id == v.Upvoter.Id && x.Post.Id == v.Post.Id))
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
            return await Task.FromResult(_appDbContext.Votes.Include(p => p.Upvoter).Include(p => p.Post));
        }

        public async Task DelAsync(int id)
        {
            try
            {
                _appDbContext.Votes.Remove(_appDbContext.Votes.FirstOrDefault(x => x.Id == id));
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
                _appDbContext.Votes.RemoveRange(_appDbContext.Votes.Include(p => p.Post).Where(x => x.Post.Id == postId));
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task DelPostVote(int postId, int userId)
        {
            try
            {
                _appDbContext.Votes.Remove(_appDbContext.Votes.Include(p => p.Post).Include(p => p.Upvoter).FirstOrDefault(x => x.Post.Id == postId && x.Upvoter.Id == userId));
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
            return await Task.FromResult(_appDbContext.Votes.Include(p => p.Post).Include(p => p.Upvoter).FirstOrDefault(x => x.Id == id));
        }

        public async Task<Votes> GetPostVotes(int postId)
        {
            return await Task.FromResult(_appDbContext.Votes.Include(p => p.Post).Include(p => p.Upvoter).FirstOrDefault(x => x.Post.Id == postId));
        }
    }
}
