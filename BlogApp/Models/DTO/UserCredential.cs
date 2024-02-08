using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models.DTO
{
    public class UserCredential
    {
		[Required(ErrorMessage = "User name is required")]
		public string Username { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required")]
		public string Password { get; set; } = string.Empty;
    }
}
