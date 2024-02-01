using BlogApp.Data;
using BlogApp.Models;

namespace BlogApp.Authentication
{
	public class UserService
	{


		private readonly ApplicationDbContext _db;

		public UserService(ApplicationDbContext db)
		{
			_db = db;
		}

		public User GetUserById(string struserId)

		{
			if (!Guid.TryParse(struserId, out Guid userId))
			{
				// Handle invalid GUID format
				throw new UnauthorizedAccessException("Unauthorized. Please provide a valid userId.");
			}

			return _db.Users.FirstOrDefault(u => u.UserId == userId);
		}
	}
}

