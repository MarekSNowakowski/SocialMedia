using SocialMedia.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Repositories
{
    public interface IPostRepository
    {
        public Task<IEnumerable<Post>> BrowseAllAsync();
        public Task<Post> GetAsync(int id);
        public Task AddAsync(Post s);
        public Task DelAsync(int id);
        public Task UpdateAsync(Post s);
    }
}
