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

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public static IEnumerable<CommentDTO> MapComments(IEnumerable<Comment> comments)
        {
            return comments.Select(x => new CommentDTO()
            {
                Id = x.Id,
                Content = x.Content,
                Post = x.Post,
                Time = x.Time,
                Author = x.Author
            });
        }

        public static CommentDTO MapComment(Comment comment)
        {
            return new CommentDTO()
            {
                Id = comment.Id,
                Content = comment.Content,
                Post = comment.Post,
                Time = comment.Time,
                Author = comment.Author
            };
        }

        public static Comment MapComment(CommentDTO comment)
        {
            return new Comment()
            {
                Id = comment.Id,
                Content = comment.Content,
                Post = comment.Post,
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
                Post = comment.Post,
                Author = comment.Author,
                Time = DateTime.Now
            };

            await _commentRepository.AddAsync(newComment);
        }

        public async Task DeleteCommentAsync(int id)
        {
            await _commentRepository.DelAsync(id);
        }

        public async Task EditCommentAsync(int id, CreateComment comment)
        {
            Comment updateComment = await _commentRepository.GetAsync(id);
            updateComment.Content = comment.Content;

            await _commentRepository.UpdateAsync(updateComment);
        }
    }
}
