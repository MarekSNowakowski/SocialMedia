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
    public class VotesService : IVotesService
    {
        private readonly IVotesRepository _votesRepository;
        private readonly IPostRepository _postRepository;

        public VotesService(IVotesRepository votesRepository, IPostRepository postRepository)
        {
            _votesRepository = votesRepository;
            _postRepository = postRepository;
        }

        public static IEnumerable<VotesDTO> MapVotes(IEnumerable<Votes> votes)
        {
            return votes.Select(x => new VotesDTO()
            {
                Id = x.Id,
                PostId = x.Post.Id,
                Upvoters = x.Upvoters
            });
        }

        public static VotesDTO MapVotes(Votes x)
        {
            VotesDTO votes = new VotesDTO()
            {
                Id = -1,
                PostId = -1,
                Upvoters = null
            };

            if(x != null)
            {
                votes.Id = x.Id;
                votes.PostId = x.Post.Id;
                votes.Upvoters = x.Upvoters;
            }

            return votes;
        }

        public async Task<IEnumerable<VotesDTO>> BrowseAllAsync()
        {
            var z = await _votesRepository.BrowseAllAsync();
            if (z != null)
                return MapVotes(z);
            else
                return null;
        }

        public async Task<VotesDTO> GetVotesAsync(int id)
        {
            Votes z = await _votesRepository.GetAsyncById(id);
            if (z != null)
                return MapVotes(z);
            else
                return null;
        }

        public async Task<VotesDTO> GetPostsVotesAsync(int postId)
        {
            Votes z = await _votesRepository.GetPostVotes(postId);
            if (z != null)
                return MapVotes(z);
            else
                return null;
        }

        public async Task AddVotesAsync(int postId)
        {
            try
            {
                var post = await _postRepository.GetAsync(postId);
                if (post == null) throw new NullReferenceException();

                Votes newVote = new Votes()
                {
                    Post = post,
                    PostId = postId,
                    Upvoters = new List<UserData>()
                };

                await _votesRepository.AddAsync(newVote);
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task UpvotePostAsync(int postId, UserDataDTO userData)
        {
            await _votesRepository.UpvotePost(postId, UserDataService.MapUserData(userData));
        }

        public async Task DeletePostVotesAsync(int postId)
        {
            await _votesRepository.DelPostVotes(postId);
        }

        public async Task DeleteVotesAsync(int id)
        {
            await _votesRepository.DelAsync(id);
        }
    }
}
