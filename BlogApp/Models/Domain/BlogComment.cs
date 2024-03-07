using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BlogApp.Models.Domain
{
	public class BlogComment
	{

		public Guid BlogCommentId { get; set; }

		public virtual Blog? Blog { get; set; }

		public string? CommentDesc { get; set; }
		public virtual User? CreatedBy { get; set; }

		[DefaultValue(true)]
		public bool IsActive { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime CreatedAt { get; set; }


		public Guid? Parent_commentID { get; set; }
		public Guid ModifiedBy { get; set; }
		[DataType(DataType.DateTime)]
		public DateTime ModifiedAt { get; set; }

		public virtual ICollection<BlogCommentLike>? Likes { get; }
	}
}
