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
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        private IEnumerable<PostDTO> MapPosts(IEnumerable<Post> posts)
        {
            return posts.Select(x => new PostDTO()
            {
                Id = x.Id,
                Title = x.Title,
                Time = x.Time,
                PhotoPath = x.PhotoPath,
                Author = x.Author
            });
        }

        private PostDTO MapPost(Post post)
        {
            var postDTO = new PostDTO()
            {
                Id = post.Id,
                Title = post.Title,
                Time = post.Time,
                PhotoPath = post.PhotoPath,
                Author = post.Author
            };

            return postDTO;
        }

        public async Task<IEnumerable<PostDTO>> BrowseAllAsync()
        {
            var z = await _postRepository.BrowseAllAsync();
            if (z != null)
                return MapPosts(z);
            else
                return null;
        }

        public async Task<PostDTO> GetPostAsync(int id)
        {
            Post z = await _postRepository.GetAsync(id);
            if (z != null)
                return MapPost(z);
            else
                return null;
        }

        public async Task AddPostAsync(CreatePost post)
        {
            Post newPost = new Post()
            {
                Title = post.Title,
                PhotoPath = post.PhotoPath,
                Author = post.Author,
                Time = DateTime.Now
            };

            await _postRepository.AddAsync(newPost);
        }

        public async Task DeletePostAsync(int id)
        {
            await _postRepository.DelAsync(id);
        }

        public async Task EditPostAsync(int id, CreatePost post)
        {
            Post updatePost = await _postRepository.GetAsync(id);
            updatePost.Title = post.Title;
            updatePost.PhotoPath = post.PhotoPath;

            await _postRepository.UpdateAsync(updatePost);
        }
    }
}
