using SocialMedia.Infrastructure.Commands;
using SocialMedia.Infrastructure.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDTO>> BrowseAllAsync();
        Task<CommentDTO> GetCommentAsync(int id);
        Task AddCommentAsync(CreateComment post);
        Task EditCommentAsync(int id, CreateComment post);
        Task DeleteCommentAsync(int id);
    }
}
