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
        Task AddCommentAsync(CreateComment comment);
        Task EditCommentAsync(int id, EditComment comment);
        Task DeleteCommentAsync(int id);
    }
}
