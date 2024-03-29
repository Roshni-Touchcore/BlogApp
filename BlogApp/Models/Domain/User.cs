﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BlogApp.Models.Domain
{

    public class User
    {

        public Guid UserId { get; set; }

        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [NotMapped]
        public string? Password { get; set; }

		public string? PasswordSalt { get; set; }
		public string? PasswordHash { get; set; }

		[Required(ErrorMessage = "Full Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string FullName { get; set; }


        [StringLength(500, ErrorMessage = "Bio cannot be longer than 500 characters.")]
        public string? Bio { get; set; }


        public string? Location { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }


        public Guid CreatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        public Guid ModifiedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ModifiedAt { get; set; }


        public virtual ICollection<Blog>? Blogs { get; }

    }
}
