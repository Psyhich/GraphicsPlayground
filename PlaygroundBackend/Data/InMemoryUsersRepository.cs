using Playground.Models;

namespace Playground.Data;

public class InMemoryUsersRepository : IPlaygroundUsersRepository
{
	public PlaygroundUser GetUser(string username)
	{
		var foundUser = FindUser(username);
		if (foundUser.Equals(PlaygroundUser.Invalid))
		{
			throw new UserDoesntExist();
		}
		return foundUser;
	}

	public List<PlaygroundUser> GetAllUsers()
	{
		return m_users;
	}

	public bool IsOwnedByUser(string projectHash, string username)
	{
		var foundUser = FindUser(username);
		if (foundUser.Equals(PlaygroundUser.Invalid))
		{
			throw new UserDoesntExist();
		}

		return foundUser.projects.Exists(project => project == projectHash);
	}

	public void RegisterUser(string username)
	{
		if (UserExists(username))
		{
			throw new UserAlreadyExists();
		}
		var newUser = new PlaygroundUser();
		newUser.name = username;
		m_users.Add(newUser);
	}

	public void DeleteUser(string username)
	{
		var foundUser = FindUser(username);
		if (foundUser.Equals(PlaygroundUser.Invalid))
		{
			throw new UserDoesntExist();
		}
		m_users.Remove(foundUser);
	}

	public void AddProjectToUser(string projectHash, string username)
	{
		int foundUserIndex = m_users.FindIndex(user => username == user.name);
		if (foundUserIndex == -1)
		{
			throw new UserDoesntExist();
		}
		if (m_users[foundUserIndex].projects.Exists(hash => hash == projectHash))
		{
			throw new ProjectAlreadyExists();
		}

		m_users[foundUserIndex].projects.Add(projectHash);
	}

	public void RemoveProjectFromUser(string projectHash, string username)
	{
		int foundUserIndex = m_users.FindIndex(user => username == user.name);
		if (foundUserIndex == -1)
		{
			throw new UserDoesntExist();
		}
		if (!m_users[foundUserIndex].projects.Exists(hash => hash == projectHash))
		{
			throw new ProjectDoesntExists();
		}

		m_users[foundUserIndex].projects.Remove(projectHash);
	}

	private bool UserExists(string name)
	{
		return m_users.Exists(user => user.name == name);
	}

	private PlaygroundUser FindUser(string username)
	{
		return m_users.Find(user => user.name == username);
	}

	private List<PlaygroundUser> m_users = new List<PlaygroundUser>();
}
