using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Domain;
using SocialMedia.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _appDbContext;

        public PostRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Post s)
        {
            try
            {
                _appDbContext.Post.Add(s);
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task<IEnumerable<Post>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.Post.Include(p => p.Author).Include(p => p.Comments).ThenInclude(p => p.Author));
        }

        public async Task DelAsync(int id)
        {
            try
            {
                _appDbContext.Post.Remove(_appDbContext.Post.FirstOrDefault(x => x.Id == id));
                // Remove all coments under the post
                _appDbContext.Comment.RemoveRange(_appDbContext.Comment.Where(x => x.Post.Id == id));

                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task<Post> GetAsync(int id)
        {
            return await Task.FromResult(_appDbContext.Post.Include(p=> p.Author).Include(p=>p.Comments).FirstOrDefault(x => x.Id == id));
        }

        public async Task UpdateAsync(Post s)
        {
            try
            {
                var z = _appDbContext.Post.FirstOrDefault(x => x.Id == s.Id);

                z.Title = s.Title;
                z.PhotoPath = s.PhotoPath;
                z.Comments = s.Comments;

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
