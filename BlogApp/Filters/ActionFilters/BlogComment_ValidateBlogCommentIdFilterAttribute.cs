using BlogApp.Data;
using BlogApp.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApp.Filters.ActionFilters
{
	public class BlogComment_ValidateBlogCommentIdFilterAttribute:ActionFilterAttribute
	{

		private readonly ApplicationDbContext db;
		public BlogComment_ValidateBlogCommentIdFilterAttribute(ApplicationDbContext db)
		{
			this.db = db;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);


			var id = context.ActionArguments["id"] as string;
			
			if (!Guid.TryParse(id, out Guid blogCommentId))
			{
				context.ModelState.AddModelError("BlogCommentId", "Invalid blog comment ID");
				var problemDetails = new ValidationProblemDetails(context.ModelState)
				{
					Status = StatusCodes.Status400BadRequest
				};
				context.Result = new BadRequestObjectResult(problemDetails);
			}

			var blogCommentExist = db.BlogComments.Find(blogCommentId);
			if (blogCommentExist == null)
			{
				context.ModelState.AddModelError("BlogComment", "Blog Comment doesn't exist.");
				var problemDetails = new ValidationProblemDetails(context.ModelState)
				{
					Status = StatusCodes.Status404NotFound
				};
				context.Result = new NotFoundObjectResult(problemDetails);
			}

			var blog = db.Blogs.Find(blogCommentExist.Blog.BlogId);
			if (blog == null)
			{
				context.ModelState.AddModelError("BlogId", "Blog doesn't exist.");
				var problemDetails = new ValidationProblemDetails(context.ModelState)
				{
					Status = StatusCodes.Status404NotFound
				};
				context.Result = new NotFoundObjectResult(problemDetails);
			}else
			{
				context.HttpContext.Items["blog"] = blog;
				context.HttpContext.Items["comment"] = blogCommentExist;
			}

		}
	}
}
