using System.Reflection.Metadata;
using BlogApp.Data;
using BlogApp.Filters.ActionFilters;
using BlogApp.Filters.AuthFilters;
using BlogApp.Models.Domain;
using BlogApp.Repository.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
	[Route("blog")]
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


		[HttpPost("like/{id}")]
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

	

	
		[HttpPost("comment/{id}")]
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


		[HttpPut("comment/update/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(BlogComment_ValidateBlogCommentIdFilterAttribute))]
		[TypeFilter(typeof(BlogComment_ValidateUpdateBlogCommentFilterAttribute))]
		public IActionResult UpdateBlogComment(string id, [FromBody] BlogComment comment)
		{
			var interactedBlog = HttpContext.Items["blog"] as Blog;
			var commentToUpdate = HttpContext.Items["comment"] as BlogComment;
			User user = userFromToken.GetUserById(HttpContext.Items["UserId"] as string);

			if (comment.CommentDesc == null)
			{
				return BadRequest("Comment  is required.");
			}
			commentToUpdate.CommentDesc = comment.CommentDesc;
			commentToUpdate.Blog = interactedBlog;
			commentToUpdate.ModifiedBy = user.UserId;
			commentToUpdate.IsActive = true;
			commentToUpdate.ModifiedAt = DateTime.Now;

			db.SaveChanges();
			return Ok(commentToUpdate);
		}



		[HttpDelete("comment/delete/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(BlogComment_ValidateBlogCommentIdFilterAttribute))]
		public IActionResult DeleteBlogComment(string id)
		{
			var commentToUpdate = HttpContext.Items["comment"] as BlogComment;

			commentToUpdate.IsActive = false;
			db.SaveChanges();

			return Ok(commentToUpdate);
		}





		[HttpPost("comment/like/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(BlogComment_ValidateBlogCommentIdFilterAttribute))]
		public IActionResult BlogCommentLikeOrUnlike(string id)
		{
			var interactedBlog = HttpContext.Items["blog"] as Blog;
			var interactedComment = HttpContext.Items["comment"] as BlogComment;
			User user = userFromToken.GetUserById(HttpContext.Items["UserId"] as string);
			

			var likedOrNot = db.BlogCommentLikes.FirstOrDefault(x => x.Blog.BlogId == interactedBlog.BlogId && x.Comment.BlogCommentId== interactedComment.BlogCommentId);
			if (likedOrNot == null)
			{
				likedOrNot = new BlogCommentLike
				{
					BlogCommentLikeId = Guid.NewGuid(),
					Blog = interactedBlog,
					Comment=interactedComment,
					CreatedBy = user,
					ModifiedBy = user.UserId,
					IsActive = true,
					CreatedAt = DateTime.Now,
					ModifiedAt = DateTime.Now
				};

				db.BlogCommentLikes.Add(likedOrNot);

			}
			else
			{
				likedOrNot.IsActive = !likedOrNot.IsActive;

			}
			db.SaveChanges();
			return Ok(likedOrNot);

		}


	}
}
