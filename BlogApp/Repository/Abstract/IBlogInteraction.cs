using BlogApp.Models.Domain;
using BlogApp.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Repository.Abstract
{
	public interface IBlogInteraction
	{

		public BlogLike LikeOrUnlike();
		public BlogCommentLike BlogCommentLikeOrUnlike();
		public List<BlogComment> GetReplyComments(string id);
		public BlogComment AddBlogComment(BlogCommentDetails comment);
		public BlogComment UpdateBlogComment(BlogCommentDetails comment);
		public BlogComment DeleteBlogComment();

	}
}
