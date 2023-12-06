using System.Text;
using Playground.Models;

namespace Playground.Data;

public class ProjectsRepository : IProjectRepository
{
	public ProjectsRepository(string directoryPath)
	{
		m_directoryPath = directoryPath;
		Directory.CreateDirectory(directoryPath);
	}

	public ProjectData Save(ProjectData data)
	{
		if (string.IsNullOrEmpty(data.name))
		{
			throw new ProjectHasInvalidData("Empty name");
		}

		string projectPath = "";
		do
		{
			data.CalculateHash();
			projectPath = Path.Join(m_directoryPath, data.hash);

		} while(Path.Exists(projectPath));

		WriteFiles(data);

		File.WriteAllText(Path.Join(projectPath, NAME_FILE), data.name);
		if (!string.IsNullOrEmpty(data.description))
		{
			File.WriteAllText(Path.Join(projectPath, DESCRIPTION_FILE), data.description);
		}
		if (data.thumbnail != null && data.thumbnail.Length > 0)
		{
			File.WriteAllText(Path.Join(projectPath, THUMBNAIL_FILE), data.thumbnail);
		}

		return data;
	}

	public List<ProjectData> GetProjectsSince(DateTime dateSince)
	{
		var projects = new List<ProjectData>();
		foreach (var dir in Directory.GetDirectories(m_directoryPath))
		{
			var info = new DirectoryInfo(dir);
			if (info.LastWriteTime >= dateSince)
			{
				projects.Add(GetByHash(info.Name));
			}
		}

		return projects;
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

		throw new NotImplementedException();
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

		data.name = File.ReadAllText(Path.Join(projectPath, NAME_FILE));
		if (File.Exists(Path.Join(projectPath, DESCRIPTION_FILE)))
		{
			data.description = File.ReadAllText(Path.Join(projectPath, DESCRIPTION_FILE));
		}
		if (File.Exists(Path.Join(projectPath, THUMBNAIL_FILE)))
		{
			data.thumbnail = File.ReadAllText(Path.Join(projectPath, THUMBNAIL_FILE));
		}
		
		return data;
	}

	private void WriteFiles(ProjectData data)
	{
		string projectPath = Path.Join(m_directoryPath, data.hash);
		Directory.CreateDirectory(projectPath);
		foreach (var (name, code) in data.files)
		{
			if (name.Split("/").Any(key => key.StartsWith(".")))
			{
				throw new ProjectHasInvalidData("It's not allowed to create dot files or folders");
			}
			if (name.Contains("/"))
			{
				int lastSeparator = name.LastIndexOf("/");
				string dirs = name.Substring(0, lastSeparator);
				CreateDirectoryRecursively(projectPath, dirs);
			}

			File.WriteAllBytes(Path.Join(projectPath, name),
				Encoding.UTF8.GetBytes(code));
		}
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
			if (fileName.StartsWith("."))
			{
				continue;
			}
			if (!String.IsNullOrEmpty(path))
			{
				fileName = Path.Join(append, fileName);
			}

			readFiles[fileName] = File.ReadAllText(Path.Join(searchPath, fileName));
		}

		foreach (var path in Directory.GetDirectories(fullSearchPath))
		{
			string directoryName = new DirectoryInfo(path).Name;
			if (directoryName.StartsWith("."))
			{
				continue;
			}
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

	private static readonly string NAME_FILE = ".name";
	private static readonly string DESCRIPTION_FILE = ".description";
	private static readonly string	THUMBNAIL_FILE = ".thumbnail";
}
