using BlogApp.Models.Domain;
using BlogApp.Models.DTO;

namespace BlogApp.Repository.Abstract
{
	public interface IBlogService
	{
		public List<Blog> GetAllBlogs();
		public Blog GetBlog();
		public Blog DeleteBlog();
		public void UpdateBlog(BlogDetails blog);

		public Blog CreateBlog(BlogDetails blog);
	}
}
