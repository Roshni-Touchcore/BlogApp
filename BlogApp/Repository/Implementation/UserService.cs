using System.Reflection.Metadata;
using BlogApp.Data;
using BlogApp.Models.Domain;
using BlogApp.Models.DTO;
using BlogApp.Repository.Abstract;
using BlogApp.Repository.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Repository.Implementation
{
    public class UserService : IUserService
    {


		private readonly ApplicationDbContext db;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly int _iteration = 3;
		private readonly string _pepper;


		public UserService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
			this.db = db;
			this._httpContextAccessor = httpContextAccessor;
			this._pepper = Environment.GetEnvironmentVariable("PasswordHashExamplePepper");

		}

        public User GetUserById(string struserId)

        {
            if (!Guid.TryParse(struserId, out Guid userId))
            {

                throw new UnauthorizedAccessException("Unauthorized. Please provide a valid userId.");
            }

            return db.Users.FirstOrDefault(u => u.UserId == userId);
        }

		public List<User> GetUsers()
        {
            return db.Users.ToList();
        }

		public User CreateUser(UserDetails user)
        {

			if (string.IsNullOrEmpty(user.Password))
			{
				throw new Exception("Password is required");


			}
			else if (user.Password.Length < 6)
			{
				throw new Exception("Password must be at least 6 characters long");
			}


			var newUser = new User
			{
				UserId = Guid.NewGuid(),
				UserName = user.UserName,
				FullName = user.FullName,
				Email = user.Email,
				IsActive = true,
				CreatedAt = DateTime.Now,
				ModifiedAt = DateTime.Now,
				PasswordSalt = PasswordHasher.GenerateSalt()
			};
			newUser.PasswordHash = PasswordHasher.ComputeHash(user.Password, newUser.PasswordSalt, _pepper, _iteration);
			newUser.CreatedBy = newUser.ModifiedBy = newUser.UserId;

			db.Users.Add(newUser);
			db.SaveChanges();

			return newUser;
		}
		public void UpdateUser(UserDetails user)
		{

			var userToUpdate = _httpContextAccessor.HttpContext.Items["user"] as User;
			userToUpdate.Email = user.Email;
			userToUpdate.FullName = user.FullName;
			userToUpdate.UserName = user.UserName;

			userToUpdate.ModifiedAt = DateTime.Now;

			if (user.Bio != null)
			{
				userToUpdate.Bio = user.Bio;
			}
			if (user.Location != null)
			{
				userToUpdate.Location = user.Location;
			}


			db.SaveChanges();

		}

		public User CurrentUser()


		{

			User user = GetUserById(_httpContextAccessor.HttpContext.Items["UserId"] as string);
			return user;
		}
			public User DeleteUser()
        {
			var userToDelete = _httpContextAccessor.HttpContext.Items["user"] as User;

			userToDelete.IsActive = false;
			db.SaveChanges();


			return userToDelete;

		}
	}
}

