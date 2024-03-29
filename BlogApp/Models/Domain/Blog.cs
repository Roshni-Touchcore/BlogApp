﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models.Domain
{
    public class Blog
    {


        public Guid BlogId { get; set; }



        //[ForeignKey("User")]
        public virtual User? CreatedBy { get; set; }

        [Required(ErrorMessage = "Blog Title is required")]
        public string Title { get; set; }



        [Required(ErrorMessage = "Blog Content is required")]
        public string Content { get; set; }

        [NotMapped]
        public IFormFile? CoverPhotoFile { get; set; }

        public string? CoverPhoto { get; set; }




        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        public Guid ModifiedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ModifiedAt { get; set; }

		public virtual ICollection<BlogLike>? Likes { get; }
		public virtual ICollection<BlogComment>? Comments { get; }

	}



    /*public class BlogCoverPhoto
	{
		[Key]
		public int BlogCoverPhotoId { get; set; }
		public string UserId { get; set; }
		public string BlogId { get; set; }
		public string ImageName { get; set; }
		public string ImagePath { get; set; }
	}*/

}
