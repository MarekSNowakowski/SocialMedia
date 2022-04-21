using SocialMedia.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Services
{
    public interface IReportsService
    {
        Task<IEnumerable<ReportsDTO>> BrowseAllAsync();
        Task<ReportsDTO> GetReportsAsync(int id);
        Task<ReportsDTO> GetPostsReportsAsync(int postId);
        Task AddReportsAsync(int postId);
        Task ReportPostAsync(int postId, UserDataDTO userData);
        Task DeleteReportsAsync(int id);
        Task DeletePostReportsAsync(int postId);
    }
}
