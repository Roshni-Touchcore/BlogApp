using BlogApp.Migrations;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Repository.Abstract
{
	public interface IUserConnection
	{

		public Object FollowUser(string id);
		public Object AcceptFollowRequest(string id);
	}
}
