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
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserDataRepository _userDataRepository;

        public CommentService(ICommentRepository commentRepository, IPostRepository postRepository, IUserDataRepository userDataRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userDataRepository = userDataRepository;
        }

        public static IEnumerable<CommentDTO> MapComments(IEnumerable<Comment> comments)
        {
            return comments.Select(x => new CommentDTO()
            {
                Id = x.Id,
                Content = x.Content,
                PostId = x.Post.Id,
                Time = x.Time,
                Author = x.Author
            });
        }

        private CommentDTO MapComment(Comment comment)
        {
            return new CommentDTO()
            {
                Id = comment.Id,
                Content = comment.Content,
                PostId = comment.Post.Id,
                Time = comment.Time,
                Author = comment.Author
            };
        }

        public async Task<IEnumerable<CommentDTO>> BrowseAllAsync()
        {
            var z = await _commentRepository.BrowseAllAsync();
            if (z != null)
                return MapComments(z);
            else
                return null;
        }

        public async Task<CommentDTO> GetCommentAsync(int id)
        {
            Comment z = await _commentRepository.GetAsync(id);
            if (z != null)
                return MapComment(z);
            else
                return null;
        }

        public async Task AddCommentAsync(CreateComment comment)
        {
            Comment newComment = new Comment()
            {
                Content = comment.Content,
                Post = await _postRepository.GetAsync(comment.PostID),
                Author = await _userDataRepository.GetAsync(comment.AuthorID),
                Time = DateTime.Now
            };

            await _commentRepository.AddAsync(newComment);
        }

        public async Task DeleteCommentAsync(int id)
        {
            await _commentRepository.DelAsync(id);
        }

        public async Task EditCommentAsync(int id, EditComment comment)
        {
            Comment updateComment = await _commentRepository.GetAsync(id);
            updateComment.Content = comment.Content;

            await _commentRepository.UpdateAsync(updateComment);
        }
    }
}
