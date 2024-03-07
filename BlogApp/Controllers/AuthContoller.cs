using System.Text;
using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Models.DTO;
using BlogApp.Repository.Abstract;
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
		private readonly IAuthService authService;
		public AuthContoller(IAuthService auth )
		{

			this.authService = auth;
		}


		[HttpPost]
		[Route("user/login")]
		public IActionResult LoginUser([FromBody] UserCredential user)
		{

			try
			{
				var data = authService.Login(user);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return Unauthorized(ex.Message);
			}
			

		}

	}
}
