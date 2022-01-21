using SocialMedia.Infrastructure.Commands;
using SocialMedia.Infrastructure.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Services
{
    public interface IPostService
    {
        Task<IEnumerable<PostDTO>> BrowseAllAsync();
        Task<PostDTO> GetPostAsync(int id);
        Task AddPostAsync(CreatePost post);
        Task EditPostAsync(int id, EditPost post);
        Task DeletePostAsync(int id);
    }
}
