﻿using BlogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data
{
	public class ApplicationDbContext:DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{

		}
		public DbSet<User> Users { get; set; }
		public DbSet<Blog> Blogs {  get; set; }
		//public DbSet<BlogCoverPhoto> BlogCoverPhotos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


		}
	}
}
