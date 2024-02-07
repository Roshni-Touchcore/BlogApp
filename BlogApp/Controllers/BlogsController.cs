using System.Security.Claims;
using BlogApp.Data;
using BlogApp.Filters;
using BlogApp.Filters.ActionFilters;
using BlogApp.Filters.AuthFilters;
using BlogApp.Models.Domain;
using BlogApp.Repository.Abstract;
using BlogApp.Repository.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [ApiController]
	[Route("[controller]")]


	
	public class BlogsController: ControllerBase
	{


		private readonly ApplicationDbContext db;
		private readonly UserService userFromToken;
		//private readonly IWebHostEnvironment environment;
		private readonly IFileService _fileService;
		public BlogsController(ApplicationDbContext db, IFileService fs)
		{
			this.db = db;
			this.userFromToken = new UserService(db);
			this._fileService = fs;
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
		[TypeFilter(typeof(Blog_ValidateBlogIdFilterAttribute))]
		public IActionResult GetBlog(string id)
		{

			return Ok(HttpContext.Items["blog"]);
		}



		[HttpPost]
		[Route("create")]
		[User_JwtVerifyFilter]
		public IActionResult CreateBlog([FromForm] Blog blog)
		{
			   


				if (blog.CoverPhotoFile == null || blog.CoverPhotoFile.Length == 0)
				{
					return BadRequest("Cover photo is required.");
				}

				var fileResult = _fileService.SaveImage(blog.CoverPhotoFile);

				if (fileResult.Item1 == 1)
				{
					blog.CoverPhoto = fileResult.Item2; 
				}

			    User user = userFromToken.GetUserById(HttpContext.Items["UserId"] as string);
				blog.CreatedBy = user;
			    blog.ModifiedBy = user.UserId;
				blog.IsActive = true;
				blog.CreatedAt = blog.ModifiedAt = DateTime.Now;

				db.Blogs.Add(blog);
				db.SaveChanges();
				return CreatedAtAction(nameof(GetBlog),
						new { id = blog.BlogId },
										blog);
		}



		[HttpPut("update/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(Blog_ValidateBlogIdFilterAttribute))]
		[TypeFilter(typeof(Blog_ValidateUpdateBlogFilterAttribute))]

		public IActionResult UpdateBlog(string id,[FromForm] Blog blog)
		{

			try
			{

				var token = HttpContext.Items["UserId"];

				if (token == null)
				{
					return Unauthorized();
				}

				User user = userFromToken.GetUserById(token as string);

				if (user == null)
				{
					return Unauthorized();
				}

				var blogToUpdate = HttpContext.Items["blog"] as Blog;
				blogToUpdate.Title= blog.Title;
				blogToUpdate.Content = blog.Content;

				if (blog.CoverPhotoFile!=null)
				{

					var fileResult = _fileService.SaveImage(blog.CoverPhotoFile);

					if (fileResult.Item1 == 1)
					{
						blogToUpdate.CoverPhoto = fileResult.Item2;
					}

				}

				blogToUpdate.ModifiedAt = DateTime.Now;
				blog.ModifiedBy = user.UserId ;


				db.SaveChanges();



				return NoContent();

			}
			catch
			{
				return Unauthorized();
			}

		}


		[HttpDelete("[action]/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(Blog_ValidateBlogIdFilterAttribute))]
		public IActionResult DeleteBlog(string id)
		{
			var blogToDelete = HttpContext.Items["blog"] as Blog;

			blogToDelete.IsActive = false;
			db.SaveChanges();

			return Ok(blogToDelete);
		}


	}
}
