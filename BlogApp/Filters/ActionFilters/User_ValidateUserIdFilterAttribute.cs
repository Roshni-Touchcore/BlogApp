using BlogApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class User_ValidateUserIdFilterAttribute : ActionFilterAttribute
{
    private readonly ApplicationDbContext db;
    public User_ValidateUserIdFilterAttribute( ApplicationDbContext db)
    {
        this.db= db;
    }


    public override void OnActionExecuting(ActionExecutingContext context)
    {
		base.OnActionExecuting(context);
		var id = context.ActionArguments["id"] as string;
		if (!string.IsNullOrEmpty(id))
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


			var user = db.Users.Find(userId);

				if (user == null)
				{
					context.ModelState.AddModelError("UserId", "User doesn't exist.");
					var problemDetails = new ValidationProblemDetails(context.ModelState)
					{
						Status = StatusCodes.Status404NotFound
					};
					context.Result = new NotFoundObjectResult(problemDetails);
				}
				else
				{
					context.HttpContext.Items["user"] = user;
				}
			
		}
	}

	}