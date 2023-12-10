using weblog_API.Data.Dto;
using weblog_API.Dto.Comment;
using weblog_API.Models.Comment;

namespace weblog_API.Mappers;

public static class CommentMapper
{
    public static CommentDto CommentToCommentDto(Comment comment)
    {
        var commentDto = new CommentDto()
        {
            Id = comment.Id,
            CreateTime = comment.CreateTime,
            AuthorId = comment.Author.Id,
            AuthorName = comment.Author.FullName,
            DeleteDate = comment.DeleteDate,
            ModifiedDate = comment.ModifiedDate,
            SubComments = comment.SubComments.Count,
            content = comment.Content
        };
        return commentDto;
    }
}