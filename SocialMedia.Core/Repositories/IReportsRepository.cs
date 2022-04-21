using SocialMedia.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Repositories
{
    public interface IReportsRepository
    {
        public Task<IEnumerable<Reports>> BrowseAllAsync();
        public Task<Reports> GetAsyncById(int id);
        public Task<Reports> GetPostReports(int postId);
        public Task AddAsync(Reports r);
        public Task DelAsync(int id);
        public Task DelPostReports(int postId);
        public Task ReportPost(int postId, UserData userData);
    }
}
