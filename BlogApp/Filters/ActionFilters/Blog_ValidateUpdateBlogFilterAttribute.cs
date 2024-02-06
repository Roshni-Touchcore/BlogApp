using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApp.Filters.ActionFilters
{
	public class Blog_ValidateUpdateBlogFilterAttribute : ActionFilterAttribute
	{

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);


			var id = context.ActionArguments["id"] as string;
			var blog = context.ActionArguments["blog"] as Blog;

			if (!string.IsNullOrEmpty(id) && blog != null)
			{
				if (!Guid.TryParse(id, out Guid blogId))
				{
					context.ModelState.AddModelError("BlogId", "Invalid Blog ID");
					var problemDetails = new ValidationProblemDetails(context.ModelState)
					{
						Status = StatusCodes.Status400BadRequest
					};
					context.Result = new BadRequestObjectResult(problemDetails);
				}

				if (blogId != blog.BlogId)
				{
					context.ModelState.AddModelError("BlogId", "BlogId is not the same as id.");
					var problemDetails = new ValidationProblemDetails(context.ModelState)
					{
						Status = StatusCodes.Status400BadRequest
					};
					context.Result = new BadRequestObjectResult(problemDetails);
				}


			}
		}
	}
}
