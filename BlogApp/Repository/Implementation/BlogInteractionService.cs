using BlogApp.Data;
using BlogApp.Models.Domain;
using BlogApp.Models.DTO;
using BlogApp.Repository.Abstract;

namespace BlogApp.Repository.Implementation
{
	public class BlogInteractionService : IBlogInteraction
	{

		private readonly ApplicationDbContext db;
		private readonly IFileService _fileService;
		private readonly IUserService userService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly User _user;

		public BlogInteractionService(ApplicationDbContext db, IFileService fs, IUserService us, IHttpContextAccessor httpContextAccessor)
		{
			this.db = db;
			this.userService = us;
			this._fileService = fs;
			this._httpContextAccessor = httpContextAccessor;
			this._user = userService.GetUserById(_httpContextAccessor.HttpContext.Items["UserId"] as string) ;

		}


		public List<BlogComment> GetReplyComments(string id)
		{
			Guid parentId;
			if (!Guid.TryParse(id, out parentId))
			{
				throw new ArgumentException("Invalid ID format.");
			}
			return db.BlogComments
						.Where(c => c.Parent_commentID == parentId)
						.OrderBy(c => c.CreatedAt)
						.ToList();
		}
		public BlogLike LikeOrUnlike()
		{
			var interactedBlog = _httpContextAccessor.HttpContext.Items["blog"] as Blog;
	


			var likedOrNot = db.BlogLikes.FirstOrDefault(x => x.Blog.BlogId == interactedBlog.BlogId && x.CreatedBy.UserId == _user.UserId);
			if (likedOrNot == null)
			{
				likedOrNot = new BlogLike
				{
					BlogLikeId = Guid.NewGuid(),
					Blog = interactedBlog,
					CreatedBy = _user,
					ModifiedBy = _user.UserId,
					IsActive = true,
					CreatedAt = DateTime.Now,
					ModifiedAt = DateTime.Now
				};

				db.BlogLikes.Add(likedOrNot);
			}
			else
			{
				likedOrNot.IsActive = !likedOrNot.IsActive;
				likedOrNot.ModifiedAt = DateTime.Now;
				likedOrNot.ModifiedBy = _user.UserId;


			}
			db.SaveChanges();
			return likedOrNot;

		}

		public BlogCommentLike BlogCommentLikeOrUnlike()
		{
			var interactedBlog = _httpContextAccessor.HttpContext.Items["blog"] as Blog;
			var interactedComment = _httpContextAccessor.HttpContext.Items["comment"] as BlogComment;
			


			var likedOrNot = db.BlogCommentLikes.FirstOrDefault(x => x.Blog.BlogId == interactedBlog.BlogId && x.Comment.BlogCommentId == interactedComment.BlogCommentId && x.CreatedBy.UserId == _user.UserId);
			if (likedOrNot == null)
			{
				likedOrNot = new BlogCommentLike
				{
					BlogCommentLikeId = Guid.NewGuid(),
					Blog = interactedBlog,
					Comment = interactedComment,
					CreatedBy = _user,
					ModifiedBy = _user.UserId,
					IsActive = true,
					CreatedAt = DateTime.Now,
					ModifiedAt = DateTime.Now
				};

				db.BlogCommentLikes.Add(likedOrNot);

			}
			else
			{
				likedOrNot.IsActive = !likedOrNot.IsActive;
				likedOrNot.ModifiedAt = DateTime.Now;
				likedOrNot.ModifiedBy = _user.UserId;

			}
			db.SaveChanges();
			return likedOrNot;
		}

		public BlogComment AddBlogComment(BlogCommentDetails comment)
		{

			var interactedBlog = _httpContextAccessor.HttpContext.Items["blog"] as Blog;
			

			if (comment.CommentDesc == null)
			{
				throw new Exception("Comment  is required.");
			}

			var newComment = new BlogComment {
				BlogCommentId = Guid.NewGuid(),
				Blog = interactedBlog,
				CreatedBy = _user,
				CommentDesc = comment.CommentDesc,
				IsActive = true,
				CreatedAt = DateTime.Now,
				ModifiedAt = DateTime.Now,
				ModifiedBy = _user.UserId,

			};

	
			
		


			db.BlogComments.Add(newComment);

			db.SaveChanges();
			return newComment;



		}

		public BlogComment UpdateBlogComment(BlogCommentDetails comment)
		{

			var interactedBlog = _httpContextAccessor.HttpContext.Items["blog"] as Blog;
			var commentToUpdate = _httpContextAccessor.HttpContext.Items["comment"] as BlogComment;
			if (commentToUpdate.CreatedBy.UserId == _user.UserId)
			{

				if (comment.CommentDesc == null)
				{
					throw new Exception("Comment  is required.");
				}
				commentToUpdate.CommentDesc = comment.CommentDesc;
				commentToUpdate.ModifiedBy = _user.UserId;
				commentToUpdate.ModifiedAt = DateTime.Now;

				db.SaveChanges();
				return commentToUpdate;
			}
			throw new UnauthorizedAccessException();
		}

		public BlogComment DeleteBlogComment()
		{
			var commentToUpdate = _httpContextAccessor.HttpContext.Items["comment"] as BlogComment;
			if (commentToUpdate.CreatedBy.UserId == _user.UserId)
			{

				commentToUpdate.IsActive = false;
				commentToUpdate.ModifiedAt = DateTime.Now;
				commentToUpdate.ModifiedBy = _user.UserId;
				db.SaveChanges();
				return commentToUpdate;
			}

			throw new UnauthorizedAccessException();
		}
	}
}
