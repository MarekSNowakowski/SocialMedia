using SocialMedia.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Repositories
{
    public interface ICommentRepository
    {
        public Task<IEnumerable<Comment>> BrowseAllAsync();
        public Task<Comment> GetAsync(int id);
        public Task AddAsync(Comment s);
        public Task DelAsync(int id);
        public Task UpdateAsync(Comment s);
    }
}
