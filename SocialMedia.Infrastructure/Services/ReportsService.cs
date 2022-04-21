using SocialMedia.Core.Domain;
using SocialMedia.Core.Repositories;
using SocialMedia.Infrastructure.Commands;
using SocialMedia.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IReportsRepository _reportsRepository;
        private readonly IPostRepository _postRepository;

        public ReportsService(IReportsRepository reportsRepository, IPostRepository postRepository)
        {
            _reportsRepository = reportsRepository;
            _postRepository = postRepository;
        }

        public static IEnumerable<ReportsDTO> MapReports(IEnumerable<Reports> reports)
        {
            return reports.Select(x => new ReportsDTO()
            {
                Id = x.Id,
                PostId = x.Post.Id,
                Reporters = x.Reporters
            });
        }

        public static ReportsDTO MapReports(Reports x)
        {
            ReportsDTO reports = new ReportsDTO()
            {
                Id = -1,
                PostId = -1,
                Reporters = null
            };

            if (x != null)
            {
                reports.Id = x.Id;
                reports.PostId = x.Post.Id;
                reports.Reporters = x.Reporters;
            }

            return reports;
        }

        public async Task<IEnumerable<ReportsDTO>> BrowseAllAsync()
        {
            var z = await _reportsRepository.BrowseAllAsync();
            if (z != null)
                return MapReports(z);
            else
                return null;
        }

        public async Task<ReportsDTO> GetReportsAsync(int id)
        {
            Reports r = await _reportsRepository.GetAsyncById(id);
            if (r != null)
                return MapReports(r);
            else
                return null;
        }

        public async Task AddReportsAsync(int postId)
        {
            try
            {
                var post = await _postRepository.GetAsync(postId);
                if (post == null) throw new NullReferenceException();

                Reports newReports = new Reports()
                {
                    Post = post,
                    PostId = postId,
                    Reporters = new List<UserData>()
                };

                await _reportsRepository.AddAsync(newReports);
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task ReportPostAsync(int postId, UserDataDTO userData)
        {
            await _reportsRepository.ReportPost(postId, UserDataService.MapUserData(userData));
        }

        public async Task DeletePostReportsAsync(int postId)
        {
            await _reportsRepository.DelPostReports(postId);
        }

        public async Task DeleteReportsAsync(int id)
        {
            await _reportsRepository.DelAsync(id);
        }

        public async Task<ReportsDTO> GetPostsReportsAsync(int postId)
        {
            Reports r = await _reportsRepository.GetPostReports(postId);
            if (r != null)
                return MapReports(r);
            else
                return null;
        }
    }
}
