using System.Reflection.Metadata;
using BlogApp.Data;
using BlogApp.Filters.ActionFilters;
using BlogApp.Filters.AuthFilters;
using BlogApp.Models.Domain;
using BlogApp.Repository.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
	[ApiController]
	public class BlogInteractionController : ControllerBase
	{
		private readonly ApplicationDbContext db;
		private readonly UserService userFromToken;
		public BlogInteractionController(ApplicationDbContext db)
        {
			this.db = db;
			this.userFromToken = new UserService(db);
		}


		[HttpPost("blog/like/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(Blog_ValidateBlogIdFilterAttribute))]
		public IActionResult LikeOrUnlike(string id)
		{
			var interactedBlog = HttpContext.Items["blog"] as Blog;
			User user = userFromToken.GetUserById(HttpContext.Items["UserId"] as string);

			var likedOrNot = db.BlogLikes.FirstOrDefault(x => x.Blog.BlogId == interactedBlog.BlogId);
			if (likedOrNot == null)
			{
				likedOrNot = new BlogLike
				{
					BlogLikeId = Guid.NewGuid(),
					Blog=interactedBlog,
					CreatedBy = user,
					ModifiedBy = user.UserId,
					IsActive = true,
					CreatedAt = DateTime.Now,
					ModifiedAt = DateTime.Now
				};

				db.BlogLikes.Add(likedOrNot);
			}
			else
			{
				likedOrNot.IsActive = !likedOrNot.IsActive;
				
			}
			db.SaveChanges();
			return Ok(likedOrNot);

		}

		[HttpGet]
		[Route("likes/all")]
		[User_JwtVerifyFilter]
		public IActionResult GetAllLikes()
		{
			return Ok(db.BlogLikes.ToList());
		}

		[HttpPost("blog/comment/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(Blog_ValidateBlogIdFilterAttribute))]
		public IActionResult AddBlogComment(string id,[FromBody]BlogComment comment)
		{
			var interactedBlog = HttpContext.Items["blog"] as Blog;
			User user = userFromToken.GetUserById(HttpContext.Items["UserId"] as string);

			if(comment.CommentDesc == null)
			{
				return BadRequest("Comment  is required.");
			}
			comment.BlogCommentId = Guid.NewGuid();
			comment.Blog=interactedBlog;
			comment.ModifiedBy = user.UserId;
			comment.CreatedBy = user;
			comment.IsActive = true;
			comment.CreatedAt = comment.ModifiedAt = DateTime.Now;

			db.BlogComments.Add(comment);

			db.SaveChanges();
			return Ok(comment);
		}


	}
}
