using System.Text;
using Playground.Models;

namespace Playground.Data;

public class ProjectsRepository : IProjectRepository
{
	public ProjectsRepository(string directoryPath)
	{
		m_directoryPath = directoryPath;
		Directory.CreateDirectory(directoryPath);
		Console.WriteLine($"Created: {directoryPath}");
	}

	public ProjectData Save(ProjectData data)
	{
		string projectPath = "";
		do
		{
			data.CalculateHash();
			projectPath = Path.Join(m_directoryPath, data.hash);

		} while(Path.Exists(projectPath));

		Directory.CreateDirectory(projectPath);
		foreach (var (name, code) in data.files)
		{
			if (name.Contains("/"))
			{
				int lastSeparator = name.LastIndexOf("/");
				string dirs = name.Substring(0, lastSeparator);
				CreateDirectoryRecursively(projectPath, dirs);
			}

			File.WriteAllBytes(Path.Join(projectPath, name),
				Encoding.UTF8.GetBytes(code));
		}

		return data;
	}

	public void Delete(string hash)
	{
		if(Path.Exists(Path.Join(m_directoryPath, hash)))
		{
			File.Delete(Path.Join(m_directoryPath, hash));
		}
		else
		{
			throw new ProjectDoesntExists();
		}
	}

	public void Update(ProjectData editedData)
	{
		string projectPath = Path.Join(m_directoryPath, editedData.hash);
		if(!Path.Exists(projectPath))
		{
			throw new ProjectDoesntExists();
		}

		foreach (var (fileName, code) in editedData.files)
		{
			File.WriteAllBytes(Path.Join(projectPath, fileName), Encoding.UTF8.GetBytes(code));
		}
	}

	public ProjectData GetByHash(string hash)
	{
		string projectPath = Path.Join(m_directoryPath, hash); 
		if (!Path.Exists(projectPath))
		{
			throw new ProjectDoesntExists();
		}

		ProjectData data = new ProjectData();
		data.hash = hash;
		data.files = ReadDirectory(projectPath);
		
		return data;
	}

	private void CreateDirectoryRecursively(string path, string directoriesPath)
	{
		string currentPath = path;
		foreach (var dir in directoriesPath.Split(Path.PathSeparator))
		{
			currentPath = Path.Join(currentPath, dir);
			Directory.CreateDirectory(currentPath);
		}
	}

	private Dictionary<string, string> ReadDirectory(string searchPath, string append="")
	{
		string fullSearchPath = Path.Join(searchPath, append);
		Dictionary<string, string> readFiles = new Dictionary<string, string>();

		foreach (var path in Directory.GetFiles(fullSearchPath))
		{
			string fileName = Path.GetFileName(path);
			if (!String.IsNullOrEmpty(path))
			{
				fileName = Path.Join(append, fileName);
			}

			readFiles[fileName] = File.ReadAllText(Path.Join(searchPath, fileName));
		}

		foreach (var path in Directory.GetDirectories(fullSearchPath))
		{
			string directoryName = new DirectoryInfo(path).Name;
			Console.WriteLine($"Found {directoryName} dir in {searchPath}/{append}");
			if (!String.IsNullOrEmpty(append))
			{
				directoryName = Path.Join(append, directoryName);
			}
			var dict = ReadDirectory(searchPath, directoryName);
			foreach (var (key, value) in dict)
			{
				readFiles.Add(key, value);
			}
		}

		return readFiles;
	}

	private string m_directoryPath;
}
