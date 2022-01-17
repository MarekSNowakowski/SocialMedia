using SocialMedia.Core.Domain;
using SocialMedia.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _appDbContext;

        public CommentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Comment s)
        {
            try
            {
                _appDbContext.Comment.Add(s);
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task<IEnumerable<Comment>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.Comment);
        }

        public async Task DelAsync(int id)
        {
            try
            {
                _appDbContext.Comment.Remove(_appDbContext.Comment.FirstOrDefault(x => x.Id == id));
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task<Comment> GetAsync(int id)
        {
            return await Task.FromResult(_appDbContext.Comment.FirstOrDefault(x => x.Id == id));
        }

        public async Task UpdateAsync(Comment s)
        {
            try
            {
                var z = _appDbContext.Comment.FirstOrDefault(x => x.Id == s.Id);

                z.Content = s.Content;

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
