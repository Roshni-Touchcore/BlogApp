using System.Text;
using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Models.DTO;
using BlogApp.Repository.Implementation;
using BlogApp.Repository.Services;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace BlogApp.Controllers
{

    [ApiController]
	public class AuthContoller:ControllerBase
	{
		private readonly ApplicationDbContext db;
		private readonly IConfiguration configuration;
		private readonly string _pepper;
		private readonly int _iteration = 3;
		public AuthContoller(ApplicationDbContext db,IConfiguration configuration)
		{
			this.db = db;
			this.configuration = configuration;
			_pepper = Environment.GetEnvironmentVariable("PasswordHashExamplePepper");

		}


		[HttpPost]
		[Route("user/login")]
		public IActionResult LoginUser([FromBody] UserCredential user)
		{

			var existingUser = this.db.Users.FirstOrDefault(u => u.UserName == user.Username);
			if (existingUser == null)
			{

				ModelState.AddModelError("Unauthorized", "User not Found or Invalid Credentials");
				var problemDetails = new ValidationProblemDetails(ModelState)
				{
					Status = StatusCodes.Status401Unauthorized
				};
				return new UnauthorizedObjectResult(problemDetails);
			}

			var passwordHash = PasswordHasher.ComputeHash(user.Password, existingUser.PasswordSalt, _pepper, _iteration);
			if (existingUser.PasswordHash != passwordHash)
			{

				ModelState.AddModelError("Unauthorized", "User not Found or Invalid Credentials");
				var problemDetails = new ValidationProblemDetails(ModelState)
				{
					Status = StatusCodes.Status401Unauthorized
				};
				return new UnauthorizedObjectResult(problemDetails);
			}

			if (existingUser != null)
			{
				var expiresAt = DateTime.UtcNow.AddMinutes(180);
				
				return Ok(new
				{
					access_token = Authenticator.CreateToken(existingUser, expiresAt, configuration.GetValue<string>("SecretKey")),
					expires_at = expiresAt
				}); ;
			}

			else
			{
				ModelState.AddModelError("Unauthorized", "User not Found or Invalid Credentials");
				var problemDetails = new ValidationProblemDetails(ModelState)
				{
					Status = StatusCodes.Status401Unauthorized
				};
				return new UnauthorizedObjectResult(problemDetails);
			}
			

		}

	}
}
