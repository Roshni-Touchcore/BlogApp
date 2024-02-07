using BlogApp.Data;
using BlogApp.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApp.Filters.ActionFilters
{
    public class User_ValidateCreateUserFilterAttribute : ActionFilterAttribute
	{
		private readonly ApplicationDbContext db;
        public User_ValidateCreateUserFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
		{

			base.OnActionExecuting(context);
			var user = context.ActionArguments["user"] as User;

			if (user == null)
			{
				context.ModelState.AddModelError("User", "User details are required");
				var problemDetails = new ValidationProblemDetails(context.ModelState)
				{
					Status = StatusCodes.Status400BadRequest
				};
				context.Result=new BadRequestObjectResult(problemDetails);
			}
			else
			{
				var existingUserEmail = db.Users.FirstOrDefault(x =>
				!string.IsNullOrWhiteSpace(user.Email) &&
					!string.IsNullOrWhiteSpace(x.Email) &&
					x.Email.ToLower() == user.Email.ToLower()
					);

				if (existingUserEmail != null)
				{
					context.ModelState.AddModelError("User", "User with this Email already Exists");
					var problemDetails = new ValidationProblemDetails(context.ModelState)
					{
						Status = StatusCodes.Status400BadRequest
					};
					context.Result = new BadRequestObjectResult(problemDetails);
				}
				else
				{

					var existingUsername = db.Users.FirstOrDefault(x =>
				!string.IsNullOrWhiteSpace(user.UserName) &&
					!string.IsNullOrWhiteSpace(x.UserName) &&
					x.UserName.ToLower() == user.UserName.ToLower()
					);

					if (existingUsername != null)
					{
						context.ModelState.AddModelError("User", "User with this username already Exists");
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
}
