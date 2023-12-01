
namespace Playground.Models;

public struct PlaygroundUser
{
	public string name;

	public List<string> projects;


    public override bool Equals(object? obj)
	{
		return obj is PlaygroundUser user && name == user.name
			&& EqualityComparer<List<string>>.Default.Equals(projects, user.projects);
	}

    public override int GetHashCode()
    {
        return HashCode.Combine(name, projects);
    }

	static public readonly PlaygroundUser Invalid = new PlaygroundUser();
}
