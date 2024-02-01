using System.Security.Claims;
using BlogApp.Authentication;
using BlogApp.Data;
using BlogApp.Filters.ActionFilters;
using BlogApp.Filters.AuthFilters;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
	[ApiController]
	[Route("[controller]")]

	public class BlogsController: ControllerBase
	{


		private readonly ApplicationDbContext db;
		private readonly UserService _userService;
		public BlogsController(ApplicationDbContext db)
		{
			this.db = db;
			this._userService= new UserService(db);

		}

		[HttpGet]
		[Route("all")]
		[User_JwtVerifyFilter]
		public IActionResult GetAllBlogs()
		{
			return Ok(db.Blogs.ToList());
		}



		[HttpGet("[action]/{id}")]
		[User_JwtVerifyFilter]
		///[TypeFilter(typeof(User_ValidateUserIdFilterAttribute))]
		public IActionResult GetBlog(string id)
		{

			return Ok(HttpContext.Items["blog"]);
		}



		[HttpPost]
		[Route("create")]
		[User_JwtVerifyFilter]
		public IActionResult CreateBlog([FromBody] Blog blog)
		{

			try
			{
				var token = HttpContext.Items["UserId"];
				User user = _userService.GetUserById(token as string);


				if(user==null)
				{
					return Unauthorized();
				}

				blog.BlogId = Guid.NewGuid();
				blog.CreatedBy = user;
				blog.ModifiedBy = user.UserId;
				blog.IsActive = true;
				blog.CreatedAt = blog.ModifiedAt = DateTime.Now;
				this.db.Blogs.Add(blog);
				this.db.SaveChanges();
				return Ok(blog);

			}
			catch 
			{
				
				
				return Unauthorized();
			}
			
			
			
			}

		[HttpDelete("[action]/{id}")]
		[User_JwtVerifyFilter]
		//[TypeFilter(typeof(User_ValidateUserIdFilterAttribute))]
		public IActionResult DeleteBlog(string id)
		{
			var blogToDelete = HttpContext.Items["blog"] as Blog;

			blogToDelete.IsActive = false;
			db.SaveChanges();

			return Ok(blogToDelete);
		}
	}
}
