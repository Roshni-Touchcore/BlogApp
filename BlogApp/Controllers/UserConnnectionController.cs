using BlogApp.Data;
using BlogApp.Filters.ActionFilters;
using BlogApp.Filters.AuthFilters;
using BlogApp.Models.Domain;
using BlogApp.Repository.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
	[Route("user/connection")]
	[ApiController]
	public class UserConnnectionController : ControllerBase
	{

		private readonly ApplicationDbContext db;
		private readonly UserService userFromToken;
		
		public UserConnnectionController(ApplicationDbContext db)
		{
			this.db = db;
			this.userFromToken = new UserService(db);
			
		}

		private UserConnection FindExistingConnection(User requestingUser, User currentUser)
		{
			return db.UserConnections.FirstOrDefault(uc =>
				uc.FollowRequesteddBy.UserId == requestingUser.UserId &&
				uc.FollowAcceptedBy.UserId == currentUser.UserId);

		}

		[HttpPost("follow/{id}")]
		[User_JwtVerifyFilter]		
		public IActionResult FollowUser(string id)
		{			
			User currentUser = userFromToken.GetUserById(HttpContext.Items["UserId"] as string);

			if (currentUser.UserId == Guid.Parse(id))
			{
				return BadRequest("You cannot follow yourself.");
			}
			User userToAccept= userFromToken.GetUserById(id);

			var existingConnection = FindExistingConnection(currentUser, userToAccept);

			if (existingConnection != null)
			{

				if (existingConnection.IsActive)
				{
					// If the connection is active but not yet accepted, deactivate it
					if (!existingConnection.IsAccepted)
					{
						existingConnection.IsActive = false;
						db.SaveChanges();
						return Ok("The pending follow request has been canceled.");
					}
					// If the connection is already accepted, deactivate it
					else
					{
						existingConnection.IsActive = false;
						db.SaveChanges();
						return Ok("The follow Connection has been deactivated.");
					}
				}
				else
				{
					// If the connection is not active, activate it again
					existingConnection.IsActive = true;
					db.SaveChanges();
					return Ok("Followed again");
				}


			}
			else
			{

				var userConnection = new UserConnection
				{
					FollowRequesteddBy = currentUser,
					FollowAcceptedBy = userToAccept,
					IsActive = true,
					IsAccepted = false,
					CreatedBy = currentUser.UserId,
					ModifiedBy = currentUser.UserId,
					CreatedAt = DateTime.Now,
					ModifiedAt = DateTime.Now
				};

				db.UserConnections.Add(userConnection);
				db.SaveChanges();
				return Ok(userConnection);

			}
		
		}



		[HttpPost("accept/{id}")]
		[User_JwtVerifyFilter]
		public IActionResult AcceptFollowRequest(string id)
		{
			User currentUser = userFromToken.GetUserById(HttpContext.Items["UserId"] as string);
			User requestingUser = userFromToken.GetUserById(id);

			// Check if the current user is the one being requested to follow
			if (currentUser.UserId == Guid.Parse(id))
			{
				return BadRequest("You cannot accept follow requests of yours itself");
			}


			var existingConnection = FindExistingConnection(requestingUser, currentUser);
			if (existingConnection != null && existingConnection.IsActive && !existingConnection.IsAccepted)
			{
				
				existingConnection.IsAccepted = true;
				existingConnection.ModifiedBy = currentUser.UserId;
				existingConnection.ModifiedAt = DateTime.Now;

				// Create a new connection with the roles reversed and mark it as accepted
				var newConnection = new UserConnection
				{
					FollowRequesteddBy = currentUser,
					FollowAcceptedBy = requestingUser,
					IsActive = true,
					IsAccepted = true,
					CreatedBy = currentUser.UserId,
					ModifiedBy = currentUser.UserId,
					CreatedAt = DateTime.Now,
					ModifiedAt = DateTime.Now
				};

				
				db.UserConnections.Add(newConnection);
				db.SaveChanges();

				return Ok("Follow request accepted successfully.");
			}
			else
			{
				return Ok("No pending follow request found to accept.");
			}
		}


	}
}
