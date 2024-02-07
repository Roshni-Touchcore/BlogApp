using System.Reflection.Metadata;
using BlogApp.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApp.Filters.ActionFilters
{
	public class BlogComment_ValidateUpdateBlogCommentFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);
			var id = context.ActionArguments["id"] as string;
			var blogComment = context.ActionArguments["blog"] as BlogComment;

			if (!string.IsNullOrEmpty(id) && blogComment != null)
			{
				if (!Guid.TryParse(id, out Guid blogCommentId))
				{
					context.ModelState.AddModelError("BlogCommentId", "Invalid Blog Comment ID");
					var problemDetails = new ValidationProblemDetails(context.ModelState)
					{
						Status = StatusCodes.Status400BadRequest
					};
					context.Result = new BadRequestObjectResult(problemDetails);
				}

				if (blogCommentId != blogComment.BlogCommentId)
				{
					context.ModelState.AddModelError("BlogCommentId", "BlogCommentId is not the same as id.");
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
