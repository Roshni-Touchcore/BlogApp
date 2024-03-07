using BlogApp.Data;
using BlogApp.Filters.ActionFilters;
using BlogApp.Filters.AuthFilters;
using BlogApp.Models.Domain;
using BlogApp.Repository.Abstract;
using BlogApp.Repository.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
	[Route("user/connection")]
	[ApiController]
	public class UserConnnectionController : ControllerBase
	{

		
		private readonly IUserConnection connectionService;

		public UserConnnectionController(IUserConnection us)
		{
			connectionService=us;

		}

	

		[HttpPost("follow/{id}")]
		[User_JwtVerifyFilter]		
		public IActionResult FollowUser(string id)
		{
			try
			{
				var connection=connectionService.FollowUser(id);
				return Ok(connection);
			}catch (Exception ex)
			{
             return BadRequest(ex.Message);
			}
		
		}



		[HttpPost("accept/{id}")]
		[User_JwtVerifyFilter]
		public IActionResult AcceptFollowRequest(string id)
		{
			try
			{
				var connection = connectionService.AcceptFollowRequest(id);
				return Ok(connection);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}


	}
}
