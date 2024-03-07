using System.Reflection.Metadata;
using BlogApp.Data;
using BlogApp.Models.Domain;
using BlogApp.Models.DTO;
using BlogApp.Repository.Abstract;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BlogApp.Repository.Implementation
{
	public class BlogService : IBlogService

	{
		private readonly ApplicationDbContext db;
		private readonly IFileService _fileService;
		private readonly IUserService userService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly User _user;
		public BlogService(ApplicationDbContext db, IFileService fs, IUserService us, IHttpContextAccessor httpContextAccessor)
		{
			this.db = db;
			this.userService = us;
			this._fileService = fs;
			this._httpContextAccessor = httpContextAccessor;
			this._user = userService.GetUserById(_httpContextAccessor.HttpContext.Items["UserId"] as string) ?? null;
		}

		public List<Blog> GetAllBlogs()
		{
			return db.Blogs.ToList();
		}

		public Blog CreateBlog(BlogDetails blog)
		{
			if (blog.CoverPhotoFile == null || blog.CoverPhotoFile.Length == 0)
			{
				throw new Exception("Cover photo is required.");
			}

			var fileResult = _fileService.SaveImage(blog.CoverPhotoFile);

		
						
			if (fileResult.Item1 != 1)
			{
				throw new Exception("Failed to save cover photo.");
			}
		
			var newBlog = new Blog
			{
				BlogId = Guid.NewGuid(),
				CoverPhoto = fileResult.Item2,
				Title = blog.Title,
				Content = blog.Content, 
				CreatedBy = _user,
				ModifiedBy = _user.UserId,
				IsActive = true,
				CreatedAt = DateTime.Now,
				ModifiedAt = DateTime.Now
			};
			db.Blogs.Add(newBlog);
			db.SaveChanges();
			return newBlog;

		}

		public Blog GetBlog()
		{
			return _httpContextAccessor.HttpContext.Items["blog"] as Blog;
		}

		 public Blog DeleteBlog()
		{
			 

			var blogToDelete = _httpContextAccessor.HttpContext.Items["blog"] as Blog; 

			blogToDelete.IsActive = false;
			blogToDelete.ModifiedAt = DateTime.Now;
			blogToDelete.ModifiedBy = _user.UserId;
			db.SaveChanges();
			return blogToDelete;

		}

		public void UpdateBlog(BlogDetails blog)
		{


			var blogToUpdate = _httpContextAccessor.HttpContext.Items["blog"] as Blog;

			if(blogToUpdate.CreatedBy == _user)
			{
				blogToUpdate.Title = blog.Title;
				blogToUpdate.Content = blog.Content;

				if (blog.CoverPhotoFile != null)
				{

					var fileResult = _fileService.SaveImage(blog.CoverPhotoFile);

					if (fileResult.Item1 == 1)
					{
						blogToUpdate.CoverPhoto = fileResult.Item2;
					}

				}

				blogToUpdate.ModifiedAt = DateTime.Now;
				blogToUpdate.ModifiedBy = _user.UserId;
				db.SaveChanges();

			}
			else
			{
				throw new UnauthorizedAccessException("You can't update this Blog");
			}
			

		}
	}
}
