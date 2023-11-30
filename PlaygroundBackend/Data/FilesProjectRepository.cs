using System.Text;
using Playground.Models;

namespace Playground.Data;

public class FilesPlayground : IProjectRepository
{
	public FilesPlayground(string directoryPath)
	{
		m_directoryPath = directoryPath;
		Directory.CreateDirectory(directoryPath);
	}

	public ProjectData Save(ProjectData data)
	{
		string playgroundPath = "";
		do
		{
			data.CalculateHash();
			playgroundPath = Path.Join(m_directoryPath, data.hash);

		} while(Path.Exists(playgroundPath));

		Directory.CreateDirectory(playgroundPath);
		foreach (var (name, code) in data.files)
		{
			if (name.Contains("/"))
			{
				int lastSeparator = name.LastIndexOf("/");
				string dirs = name.Substring(0, lastSeparator);
				CreateDirectoryRecursively(playgroundPath, dirs);
			}

			File.WriteAllBytes(Path.Join(playgroundPath, name),
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
		string playgroundPath = Path.Join(m_directoryPath, editedData.hash);
		if(!Path.Exists(playgroundPath))
		{
			throw new ProjectDoesntExists();
		}

		foreach (var (fileName, code) in editedData.files)
		{
			File.WriteAllBytes(Path.Join(playgroundPath, fileName), Encoding.UTF8.GetBytes(code));
		}
	}

	public ProjectData GetByHash(string hash)
	{
		string playgroundPath = Path.Join(m_directoryPath, hash); 
		if (!Path.Exists(playgroundPath))
		{
			throw new ProjectDoesntExists();
		}

		ProjectData data = new ProjectData();
		data.hash = hash;
		data.files = ReadDirectory(playgroundPath);
		
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

	private Dictionary<string, string> ReadDirectory(string searchPath, string prepend="")
	{
		Dictionary<string, string> readFiles = new Dictionary<string, string>();

		foreach (var path in Directory.GetFiles(searchPath))
		{
			if (File.Exists(path))
			{
				string fileName = Path.GetFileName(path);
				if (!String.IsNullOrEmpty(path))
				{
					fileName = Path.Join(prepend, fileName);
				}

				readFiles[fileName] = File.ReadAllText(Path.Join(searchPath, fileName));
			}
			else if(Directory.Exists(path))
			{
				string directoryName = new DirectoryInfo(path).Name;
				if (!String.IsNullOrEmpty(prepend))
				{
					directoryName = Path.Join(prepend, directoryName);
				}
				var dict = ReadDirectory(path, directoryName);
				foreach (var (key, value) in dict)
				{
					readFiles.Add(key, value);
				}
			}
		}

		return readFiles;
	}

	private string m_directoryPath;
}
