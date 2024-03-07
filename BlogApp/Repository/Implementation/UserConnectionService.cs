using BlogApp.Data;
using BlogApp.Models.Domain;
using BlogApp.Repository.Abstract;

namespace BlogApp.Repository.Implementation
{
	public class UserConnectionService: IUserConnection
	{

		private readonly ApplicationDbContext db;	
		private readonly IUserService userService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly User _user;
		public UserConnectionService (ApplicationDbContext db,  IUserService us, IHttpContextAccessor httpContextAccessor)
		{
			this.db = db;
			this.userService = us;
		
			this._httpContextAccessor = httpContextAccessor;
			this._user = userService.GetUserById(_httpContextAccessor.HttpContext.Items["UserId"] as string) ?? null;
		}

		private UserConnection FindExistingConnection(User requestingUser, User currentUser)
		{
			return db.UserConnections.FirstOrDefault(uc =>
				uc.FollowRequesteddBy.UserId == requestingUser.UserId &&
				uc.FollowAcceptedBy.UserId == currentUser.UserId);

		}

		public Object FollowUser(string id)		
			{
				if (_user.UserId == Guid.Parse(id))
				{
					throw new Exception("You cannot follow yourself.");
				}

				User userToAccept = userService.GetUserById(id);

				UserConnection existingConnection = FindExistingConnection(_user, userToAccept);
				if (existingConnection != null)
				{

					if (existingConnection.IsActive)
					{
						// If the connection is active but not yet accepted, deactivate it
						if (!existingConnection.IsAccepted)
						{
							existingConnection.IsActive = false;
							db.SaveChanges();
							return existingConnection;
						}
						// If the connection is already accepted, deactivate it
						else
						{
							existingConnection.IsActive = false;
							db.SaveChanges();
							return existingConnection;
						}
					}
					else
					{
						// If the connection is not active, activate it again
						existingConnection.IsActive = true;
						db.SaveChanges();
						return existingConnection;
					}
				}
				else
				{

					var userConnection = new UserConnection
					{
						FollowRequesteddBy = _user,
						FollowAcceptedBy = userToAccept,
						IsActive = true,
						IsAccepted = false,
						CreatedBy = _user.UserId,
						ModifiedBy = _user.UserId,
						CreatedAt = DateTime.Now,
						ModifiedAt = DateTime.Now
					};

					db.UserConnections.Add(userConnection);
					db.SaveChanges();
					return userConnection;
				}
			}
		

		public Object AcceptFollowRequest(string id)
		{
			User currentUser = userService.GetUserById(_httpContextAccessor.HttpContext.Items["UserId"] as string);
			User requestingUser = userService.GetUserById(id);

			// Check if the current user is the one being requested to follow
			if (currentUser.UserId == Guid.Parse(id))
			{
				throw new Exception("You cannot accept follow requests of yours itself");
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

				throw new Exception("Follow request accepted successfully.");
			}
			else
			{
				throw new Exception("No pending follow request found to accept.");
			}
		}
	}
}
