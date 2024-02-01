using System.Text;
using BlogApp.Authentication;
using BlogApp.Data;
using BlogApp.Models;
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
		public AuthContoller(ApplicationDbContext db,IConfiguration configuration)
		{
			this.db = db;
			this.configuration = configuration;

		}


		[HttpPost]
		[Route("/login")]
		public IActionResult LoginUser([FromBody] UserCredential user)
		{

			var existingUser = this.db.Users.FirstOrDefault(u => u.UserName == user.Username && u.Password == user.Password);


			if (existingUser != null)
			{
				var expiresAt = DateTime.UtcNow.AddMinutes(90);
				
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
