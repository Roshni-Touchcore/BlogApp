using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApp.Filters.ActionFilters
{
	public class User_ValidateUpdateUserFilterAttribute: ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

			var id = context.ActionArguments["id"] as  string;
			var user = context.ActionArguments["user"] as User;

			if (!string.IsNullOrEmpty(id) && user!=null)
			{
				if (!Guid.TryParse(id, out Guid userId))
				{
					context.ModelState.AddModelError("UserId", "Invalid User ID");
					var problemDetails = new ValidationProblemDetails(context.ModelState)
					{
						Status = StatusCodes.Status400BadRequest
					};
					context.Result = new BadRequestObjectResult(problemDetails);
				}

				if(userId != user.UserId)
				{
					context.ModelState.AddModelError("UserId", "UserId is not the same as id.");
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
