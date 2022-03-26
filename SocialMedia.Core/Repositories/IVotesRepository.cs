using SocialMedia.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Repositories
{
    public interface IVotesRepository
    {
        public Task<IEnumerable<Votes>> BrowseAllAsync();
        public Task<Votes> GetAsyncById(int id);
        public Task<Votes> GetPostVotes(int postId);
        public Task AddAsync(Votes v);
        public Task DelAsync(int id);
        public Task DelPostVotes(int postId);
        public Task UpvotePost(int postId, UserData userData);
    }
}