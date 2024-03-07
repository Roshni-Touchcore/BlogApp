using System;
using BlogApp.Data;
using BlogApp.Models.DTO;
using BlogApp.Repository.Abstract;
using BlogApp.Repository.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Repository.Implementation
{
	public class AuthService:IAuthService
	{

		private readonly ApplicationDbContext db;
		private readonly IConfiguration configuration;
		private readonly string _pepper;
		private readonly int _iteration = 3;

		public AuthService(ApplicationDbContext db, IConfiguration configuration)
		{
			this.db = db;
			this.configuration = configuration;
			_pepper = Environment.GetEnvironmentVariable("PasswordHashExamplePepper");

		}
		public Object Login(UserCredential user)
		{
			var existingUser = db.Users.FirstOrDefault(u => u.UserName == user.Username);
			if (existingUser == null)
			{

				throw new UnauthorizedAccessException("User not Found or Invalid Credentials");
				
			}

			var passwordHash = PasswordHasher.ComputeHash(user.Password, existingUser.PasswordSalt, _pepper, _iteration);
			if (existingUser.PasswordHash != passwordHash)
			{

				throw new UnauthorizedAccessException("User not Found or Invalid Credentials");
		
			}

			if (existingUser != null)
			{
				var expiresAt = DateTime.UtcNow.AddMinutes(180);
				var accessToken = Authenticator.CreateToken(existingUser, expiresAt, configuration.GetValue<string>("SecretKey"));

				return new ObjectResult(new
				{
					access_token = accessToken,
					expires_at = expiresAt
				});
			}

			else
			{
				throw new UnauthorizedAccessException("User not Found or Invalid Credentials");				
			}
		}
	}
}
