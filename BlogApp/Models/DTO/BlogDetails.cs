using BlogApp.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BlogApp.Models.DTO
{
	public class BlogDetails
	{


		[Required(ErrorMessage = "Blog Title is required")]
		public string Title { get; set; }



		[Required(ErrorMessage = "Blog Content is required")]
		public string Content { get; set; }

		public IFormFile? CoverPhotoFile { get; set; }

		public string? CoverPhoto { get; set; }

	}
}
