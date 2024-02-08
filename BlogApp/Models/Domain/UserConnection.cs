using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BlogApp.Models.Domain
{
	public class UserConnection
	{

		public Guid UserConnectionId { get; set; }

		public virtual User? FollowRequesteddBy { get; set; }
		public virtual User? FollowAcceptedBy { get; set; }
		[DefaultValue(false)]
		public bool IsAccepted { get; set; }

		[DefaultValue(true)]
		public bool IsActive { get; set; }

		public Guid ModifiedBy { get; set; }
		public Guid CreatedBy { get; set; }


		[DataType(DataType.DateTime)]
		public DateTime CreatedAt { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime ModifiedAt { get; set; }

	}
}
