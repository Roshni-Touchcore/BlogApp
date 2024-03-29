﻿using BlogApp.Data;
using BlogApp.Models.Domain;
using BlogApp.Repository.Abstract;
using BlogApp.Repository.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApp.Filters
{
	public class User_TokenExceptionFilterAttribute: ExceptionFilterAttribute
	{

		private readonly IUserService userService;
		private readonly ApplicationDbContext db;
		public User_TokenExceptionFilterAttribute(ApplicationDbContext db, IUserService us)
		{
			this.db = db;
			userService = us;

		}
		public override void OnException(ExceptionContext context)
		{
			base.OnException(context);

			var token = context.HttpContext.Items["UserId"];

			if (token == null)
			{
				context.ModelState.AddModelError("User", "User is UnAuthorized");
				var problemDetails = new ValidationProblemDetails(context.ModelState)
				{
					Status = StatusCodes.Status401Unauthorized
				};
				context.Result = new UnauthorizedObjectResult(problemDetails);
			}

			User user = userService.GetUserById(token as string);

			if (user == null)
			{
				context.ModelState.AddModelError("User", "User does not Exist");
				var problemDetails = new ValidationProblemDetails(context.ModelState)
				{
					Status = StatusCodes.Status404NotFound
				};
				context.Result = new NotFoundObjectResult(problemDetails);
			}

			
		}
	}
}
