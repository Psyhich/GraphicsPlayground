using Playground.Models;

namespace Playground.Data;

public interface IPlaygroundUsersRepository
{
	public PlaygroundUser GetUser(string username);

	public List<PlaygroundUser> GetAllUsers();

	public bool IsOwnedByUser(string projectHash, string username);
	public void AddProjectToUser(string projectHash, string username);
	public void RemoveProjectFromUser(string projectHash, string username);

	public void RegisterUser(string username);
	public void DeleteUser(string username);
}

class UserDoesntExist : Exception;
class UserAlreadyExists : Exception;
class ProjectAlreadyExists : Exception;
