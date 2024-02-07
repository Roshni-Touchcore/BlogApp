using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BlogApp.Models.Domain
{
	public class BlogCommentLike
	{

		public Guid BlogCommentLikeId { get; set; }

		public virtual Blog? Blog { get; set; }
		public virtual User? CreatedBy { get; set; }
		public virtual BlogComment? Comment { get; set; }

		[DefaultValue(true)]
		public bool IsActive { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime CreatedAt { get; set; }

		public Guid ModifiedBy { get; set; }
		[DataType(DataType.DateTime)]
		public DateTime ModifiedAt { get; set; }
	}
}
