using SocialMedia.Infrastructure.Commands;
using SocialMedia.Infrastructure.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Services
{
    public interface IVotesService
    {
        Task<IEnumerable<VotesDTO>> BrowseAllAsync();
        Task<VotesDTO> GetVotesAsync(int id);
        Task<VotesDTO> GetPostsVotesAsync(int postId);
        Task AddVotesAsync(int postId, int userId);
        Task DeleteVotesAsync(int id);
        Task DeletePostVoteAsync(int postId, int userId);
        Task DeletePostVotesAsync(int postId);
    }
}
