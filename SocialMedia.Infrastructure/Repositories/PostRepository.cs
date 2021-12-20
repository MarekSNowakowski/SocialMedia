using SocialMedia.Core.Domain;
using SocialMedia.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    class PostRepository : IPostRepository
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
            return await Task.FromResult(_appDbContext.Post);
        }

        public async Task DelAsync(int id)
        {
            try
            {
                _appDbContext.Post.Remove(_appDbContext.Post.FirstOrDefault(x => x.Id == id));
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
            return await Task.FromResult(_appDbContext.Post.FirstOrDefault(x => x.Id == id));
        }

        public async Task UpdateAsync(Post s)
        {
            try
            {
                var z = _appDbContext.Post.FirstOrDefault(x => x.Id == s.Id);

                z.Title = s.Title;
                z.Image = s.Image;

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
