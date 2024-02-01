using BlogApp.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApp.Filters.AuthFilters
{
	public class User_JwtVerifyFilterAttribute : Attribute, IAsyncAuthorizationFilter
	{
		public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
		{
			if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
			{

				context.Result = new UnauthorizedResult();
				return;

			}

			var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();

			var claim = Authenticator.VerifyToken(token, configuration.GetValue<string>("SecretKey"));


			if (claim == null)
			{
				context.Result = new UnauthorizedResult();
			}
		}
	}
}
