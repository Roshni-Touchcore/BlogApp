using System.ComponentModel.DataAnnotations;
using BlogApp.Authentication;
using BlogApp.Data;
using BlogApp.Filters.ActionFilters;
using BlogApp.Filters.AuthFilters;
using BlogApp.Models.Domain;
using BlogApp.Models.DTO;
using BlogApp.Repository.Abstract;
using BlogApp.Repository.Implementation;
using BlogApp.Repository.Services;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [ApiController]
	[Route("user")]
	public class UserController: ControllerBase
	{
		

		private readonly IUserService userService;
		public UserController(IUserService us)
        {
			userService = us;
		}

        [HttpGet]
		[Route("all")]
		[User_JwtVerifyFilter]
		public IActionResult GetUsers()
		{
			return Ok(userService.GetUsers());
		}



		[HttpGet("{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(User_ValidateUserIdFilterAttribute))]
		public IActionResult GetUserById(string id)
		{
			return Ok(userService.GetUserById(id));
		}

		[HttpGet]
		[User_JwtVerifyFilter]
		public IActionResult GetUser()
		{			
			return Ok(userService.CurrentUser());
		}


		[HttpPost("create")]
		[TypeFilter(typeof(User_ValidateCreateUserFilterAttribute))]
		public IActionResult CreateUser([FromBody] UserDetails user)
		{

			var newUser = userService.CreateUser(user);
			return CreatedAtAction(nameof(GetUserById),
			   new { id = newUser.UserId },
			   newUser);
		}



	    [HttpPut("update/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(User_ValidateUserIdFilterAttribute))]
		[TypeFilter(typeof(User_ValidateCreateUserFilterAttribute))]
		public IActionResult UpdateUser(string id,UserDetails user)
		{
			userService.UpdateUser(user);
			return NoContent();
		}


		[HttpDelete("delete{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(User_ValidateUserIdFilterAttribute))]
		public IActionResult DeleteUser(string id)
		{
			return Ok(userService.DeleteUser());
		}

	}
}
