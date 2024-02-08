using System.ComponentModel.DataAnnotations;
using BlogApp.Authentication;
using BlogApp.Data;
using BlogApp.Filters.ActionFilters;
using BlogApp.Filters.AuthFilters;
using BlogApp.Models.Domain;
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
		private readonly ApplicationDbContext db;
		private readonly string _pepper;
		private readonly UserService userFromToken;
		private readonly int _iteration = 3;
		public UserController(ApplicationDbContext db)
        {
            this.db=db;
			this.userFromToken = new UserService(db);
			_pepper = Environment.GetEnvironmentVariable("PasswordHashExamplePepper");

		}

        [HttpGet]
		[Route("all")]
		[User_JwtVerifyFilter]
		public IActionResult GetUsers()
		{
			return Ok(db.Users.ToList());
		}



		[HttpGet("{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(User_ValidateUserIdFilterAttribute))]
		public IActionResult GetUserById(string id)
		{

			return Ok(HttpContext.Items["user"]);
		}

		[HttpGet]
		[User_JwtVerifyFilter]
		public IActionResult GetUser()
		{
			User user = userFromToken.GetUserById(HttpContext.Items["UserId"] as string);
			return Ok(user);
		}


		[HttpPost("create")]
		[TypeFilter(typeof(User_ValidateCreateUserFilterAttribute))]
		public IActionResult CreateUser([FromBody] User user)
		{


			if (string.IsNullOrEmpty(user.Password))
			{
				return BadRequest("Password is required");


			}
			else if (user.Password.Length < 6)
			{
				return BadRequest("Password must be at least 6 characters long");
			}




			user.UserId=user.CreatedBy=user.ModifiedBy=Guid.NewGuid();
			user.CreatedAt = user.ModifiedAt = DateTime.Now;
			user.IsActive=true;
			user.PasswordSalt = PasswordHasher.GenerateSalt();
			user.PasswordHash = PasswordHasher.ComputeHash(user.Password, user.PasswordSalt, _pepper, _iteration);

			this.db.Users.Add(user);
			this.db.SaveChanges();



			return CreatedAtAction(nameof(GetUserById),
			   new { id = user.UserId },
			   user);
		}

	    [HttpPut("update/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(User_ValidateUserIdFilterAttribute))]
		[TypeFilter(typeof(User_ValidateCreateUserFilterAttribute))]
		public IActionResult UpdateUser(string id,User user)
		{
			var userToUpdate = HttpContext.Items["user"] as User;
			userToUpdate.Email = user.Email;
			userToUpdate.FullName = user.FullName;
			userToUpdate.UserName = user.UserName;

			userToUpdate.ModifiedAt= DateTime.Now;

			if (user.Bio != null)
			{
				userToUpdate.Bio = user.Bio;
			}
			if (user.Location != null)
			{
				userToUpdate.Location = user.Location;
			}


			db.SaveChanges();

			return NoContent();
		}


		[HttpDelete("delete{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(User_ValidateUserIdFilterAttribute))]
		public IActionResult DeleteUser(string id)
		{
			var userToDelete = HttpContext.Items["user"] as User;

			userToDelete.IsActive = false;
			db.SaveChanges();

			return Ok(userToDelete);
		}

	}
}
