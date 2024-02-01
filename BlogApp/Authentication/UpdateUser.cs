using BlogApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BlogApp.Authentication
{
	public class UpdateUser
	{
	

		[Required(ErrorMessage = "User name is required")]
		public string UserName { get; set; }

		[DataType(DataType.EmailAddress)]
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; }


		[Required(ErrorMessage = "Full Name is required")]
		[StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
		public string FullName { get; set; }


		[StringLength(500, ErrorMessage = "Bio cannot be longer than 500 characters.")]
		public string? Bio { get; set; }


		public string? Location { get; set; }

		[DefaultValue(true)]
		public bool IsActive { get; set; }

	}
}
