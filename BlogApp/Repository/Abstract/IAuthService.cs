using BlogApp.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Repository.Abstract
{
	public interface IAuthService
	{
		public Object Login(UserCredential user);
		
	}
}
