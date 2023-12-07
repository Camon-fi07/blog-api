using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using weblog_API.Data.Dto;
using weblog_API.Services.IServices;

namespace weblog_API.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : Controller
{
   private readonly ICommentService _commentService;

   public CommentController(ICommentService commentService)
   {
      _commentService = commentService;
   }

   [HttpGet("{postId:guid}")]
   public async Task<ActionResult<List<CommentDto>>> GetPostComments(Guid postId)
   {
      var comments = await _commentService.GetPostComments(postId);
      return Ok(comments);
   }
   
   [HttpGet("{commentId:guid}/tree")]
   public async Task<ActionResult<List<CommentDto>>> GetNestedComments(Guid commentId)
   {
      var comments = await _commentService.GetAllNestedComments(commentId);
      return Ok(comments);
   }
   
   [HttpPost("{postId:guid}/add")]
   [Authorize]
   public async Task<IActionResult> AddComment([FromBody] CreateCommentDto createCommentDto, Guid postId)
   {
      string token = HttpContext.Request.Headers["Authorization"];
      await _commentService.AddComment(postId, createCommentDto, token);
      return Ok();
   }
   
   [HttpPut("{commentId:guid}/edit")]
   [Authorize]
   public async Task<IActionResult> EditComment([FromBody] UpdateCommentDto updateCommentDto, Guid commentId)
   {
      string token = HttpContext.Request.Headers["Authorization"];
      await _commentService.EditComment(commentId, updateCommentDto, token);
      return Ok();
   }
   
   [HttpDelete("{commentId:guid}/delete")]
   [Authorize]
   public async Task<IActionResult> DeleteComment(Guid commentId)
   {
      string token = HttpContext.Request.Headers["Authorization"];
      await _commentService.DeleteComment(commentId, token);
      return Ok();
   }
   
}