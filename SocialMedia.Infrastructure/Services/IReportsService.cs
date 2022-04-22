using SocialMedia.Core.Domain;
using SocialMedia.Infrastructure.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Services
{
    public interface IReportsService
    {
        Task<IEnumerable<ReportsDTO>> BrowseAllAsync();
        Task<ReportsDTO> GetReportsAsync(int id);
        Task<ReportsDTO> GetPostsReportsAsync(int postId);
        Task AddReportsAsync(int postId, int userId);
        Task DeleteReportsAsync(int id);
        Task DeletePostReportAsync(int postId, int userId);
        Task DeletePostReportsAsync(int postId);
    }
}
