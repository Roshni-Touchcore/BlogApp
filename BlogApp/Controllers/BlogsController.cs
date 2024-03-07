using System.Security.Claims;
using BlogApp.Data;
using BlogApp.Filters;
using BlogApp.Filters.ActionFilters;
using BlogApp.Filters.AuthFilters;
using BlogApp.Models.Domain;
using BlogApp.Models.DTO;
using BlogApp.Repository.Abstract;
using BlogApp.Repository.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [ApiController]
	[Route("blog")]

	
	public class BlogsController: ControllerBase
	{


		private readonly IBlogService blogService;
		public BlogsController( IBlogService bs )
		{
		
			this.blogService = bs;
		}




		[HttpGet]
		[Route("all")]
		[User_JwtVerifyFilter]
		public IActionResult GetAllBlogs()
		{
			return Ok(blogService.GetAllBlogs());
		}



		[HttpGet("{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(Blog_ValidateBlogIdFilterAttribute))]
		public IActionResult GetBlog(string id)
		{

			return Ok(blogService.GetBlog());
		}



		[HttpPost]
		[Route("create")]
		[User_JwtVerifyFilter]
		public IActionResult CreateBlog([FromForm] BlogDetails blog) 
		{

			try
			{
				Blog newBlog = blogService.CreateBlog(blog);
				return CreatedAtAction(nameof(GetBlog), new { id = newBlog.BlogId }, newBlog);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message); 
			}
		}



		[HttpPut("update/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(Blog_ValidateBlogIdFilterAttribute))]
		public IActionResult UpdateBlog(string id,[FromForm] BlogDetails blog)
		{


			try
			{
				blogService.UpdateBlog(blog);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			
		}


		[HttpDelete("delete/{id}")]
		[User_JwtVerifyFilter]
		[TypeFilter(typeof(Blog_ValidateBlogIdFilterAttribute))]
		public IActionResult DeleteBlog(string id)
		{
			
			return Ok(blogService.DeleteBlog());
		}


	}
}
