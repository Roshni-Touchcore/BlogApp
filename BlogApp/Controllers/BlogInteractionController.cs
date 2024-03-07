using System.Reflection.Metadata;
using BlogApp.Data;
using BlogApp.Filters.ActionFilters;
using BlogApp.Filters.AuthFilters;
using BlogApp.Models.Domain;
using BlogApp.Models.DTO;
using BlogApp.Repository.Abstract;
using BlogApp.Repository.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
	[Route("blog")]
	[ApiController]

	public class BlogInteractionController : ControllerBase
	{
		
		private readonly IUserService userService;
		private readonly IBlogInteraction blogInteractionService;

		public BlogInteractionController(IUserService us, IBlogInteraction bs)
        {
			
			this.userService = us;
			this.blogInteractionService = bs;
		}


		[HttpPost("like/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(Blog_ValidateBlogIdFilterAttribute))]
		public IActionResult LikeOrUnlike(string id)
		{
			
			return Ok(blogInteractionService.LikeOrUnlike());

		}

	

	
		[HttpPost("comment/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(Blog_ValidateBlogIdFilterAttribute))]
		public IActionResult AddBlogComment(string id,[FromBody]BlogCommentDetails comment)
		{
			var newComment = blogInteractionService.AddBlogComment(comment);
			return Ok(newComment);
		}


		[HttpPost("reply/comments/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(Blog_ValidateBlogIdFilterAttribute))]
		public IActionResult GetReplyComments(string id)
		{

			return Ok(blogInteractionService.GetReplyComments(id));

		}

		[HttpPut("comment/update/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(BlogComment_ValidateBlogCommentIdFilterAttribute))]
		public IActionResult UpdateBlogComment(string id, [FromBody] BlogCommentDetails comment)
		{
			try
			{
				blogInteractionService.UpdateBlogComment(comment);
				return NoContent();

			}catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			
		}



		[HttpDelete("comment/delete/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(BlogComment_ValidateBlogCommentIdFilterAttribute))]
		public IActionResult DeleteBlogComment(string id)
		{

			try
			{
				
				return Ok(blogInteractionService.DeleteBlogComment());

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}







		[HttpPost("comment/like/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(BlogComment_ValidateBlogCommentIdFilterAttribute))]
		public IActionResult BlogCommentLikeOrUnlike(string id)
		{
		
			return Ok(blogInteractionService.BlogCommentLikeOrUnlike());

		}


	}
}
