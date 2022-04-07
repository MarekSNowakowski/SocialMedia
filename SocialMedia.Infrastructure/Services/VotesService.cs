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
        private readonly IUserDataRepository _userDataRepository;

        public VotesService(IVotesRepository votesRepository, IPostRepository postRepository, IUserDataRepository userDataRepository)
        {
            _votesRepository = votesRepository;
            _postRepository = postRepository;
            _userDataRepository = userDataRepository;
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
            return new VotesDTO()
            {
                Id = x.Id,
                PostId = x.Post.Id,
                Upvoters = x.Upvoters
            };
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
            Votes newVote = new Votes()
            {
                Post = await _postRepository.GetAsync(postId),
                Upvoters = new List<UserData>()
            };

            await _votesRepository.AddAsync(newVote);
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
