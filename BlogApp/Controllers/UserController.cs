using System.ComponentModel.DataAnnotations;
using BlogApp.Authentication;
using BlogApp.Data;
using BlogApp.Filters.ActionFilters;
using BlogApp.Filters.AuthFilters;
using BlogApp.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class UserController: ControllerBase
	{
		private readonly ApplicationDbContext db;
        public UserController(ApplicationDbContext db)
        {
            this.db=db;

        }

        [HttpGet]
		[Route("all")]
		[User_JwtVerifyFilter]
		public IActionResult GetUsers()
		{
			return Ok(db.Users.ToList());
		}



		[HttpGet("[action]/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(User_ValidateUserIdFilterAttribute))]
		public IActionResult GetUserById(string id)
		{

			return Ok(HttpContext.Items["user"]);
		}


		[HttpPost]
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

			this.db.Users.Add(user);
			this.db.SaveChanges();



			return CreatedAtAction(nameof(GetUserById),
			   new { id = user.UserId },
			   user);
		}

	    [HttpPut("[action]/{id}")]
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


		[HttpDelete("[action]/{id}")]
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
