using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApp.Filters.ActionFilters
{
	public class Blog_ValidateBlogIdFilterAttribute : ActionFilterAttribute
	{
		private readonly ApplicationDbContext db;
		public Blog_ValidateBlogIdFilterAttribute(ApplicationDbContext db)
		{
			this.db = db;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);
			var id = context.ActionArguments["id"] as string;
			if (!Guid.TryParse(id, out Guid blogId))
			{
				context.ModelState.AddModelError("BlogId", "Invalid blog ID");
				var problemDetails = new ValidationProblemDetails(context.ModelState)
				{
					Status = StatusCodes.Status400BadRequest
				};
				context.Result = new BadRequestObjectResult(problemDetails);
			}

			var blog = db.Blogs.Find(blogId);
			if (blog == null)
			{
				context.ModelState.AddModelError("BlogId", "Blog doesn't exist.");
				var problemDetails = new ValidationProblemDetails(context.ModelState)
				{
					Status = StatusCodes.Status404NotFound
				};
				context.Result = new NotFoundObjectResult(problemDetails);
			}
			else
			{
				context.HttpContext.Items["blog"] = blog;
			}

		}

	}
}
