using BlogApp.Models.Domain;
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
		public DbSet<BlogLike> BlogLikes { get; set; }
		public DbSet<BlogComment> BlogComments { get; set; }
		public DbSet<BlogCommentLike> BlogCommentLikes { get; set; }
		public DbSet<UserConnection> UserConnections { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


		}
	}
}
