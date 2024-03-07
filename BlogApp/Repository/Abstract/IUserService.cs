using BlogApp.Models.Domain;
using BlogApp.Models.DTO;

namespace BlogApp.Repository.Abstract
{
	public interface IUserService

	{
		public List<User> GetUsers();
		public User GetUserById(string struserId);
		public User CurrentUser();
		public User DeleteUser();
		public void UpdateUser(UserDetails user);

		public User CreateUser(UserDetails user);

	}
}
