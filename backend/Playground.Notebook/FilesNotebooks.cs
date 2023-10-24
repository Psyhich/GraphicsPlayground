using System.Text;

namespace Playground.Notebook;

public class FilesNotebook : INotebookRepository
{
	public FilesNotebook(string directoryPath)
	{
		m_directoryPath = directoryPath;
		Directory.CreateDirectory(directoryPath);
	}

	public NotebookData Create(string code)
	{
		NotebookData newData = new NotebookData(code);
		while(Path.Exists(Path.Join(m_directoryPath, newData.hash)))
		{
			newData.hash = NotebookData.GetHash(code + newData.hash);
		}

		File.WriteAllBytes(Path.Join(m_directoryPath, newData.hash), Encoding.UTF8.GetBytes(newData.code));

		return newData;
	}

	public void Delete(string hash)
	{
		if(Path.Exists(Path.Join(m_directoryPath, hash)))
		{
			File.Delete(Path.Join(m_directoryPath, hash));
		}
		else
		{
			throw new NotebookDoesntExists();
		}
	}

	public void Edit(NotebookData editedData)
	{
		if(Path.Exists(Path.Join(m_directoryPath, editedData.hash)))
		{
			File.WriteAllBytes(Path.Join(m_directoryPath, editedData.hash), Encoding.UTF8.GetBytes(editedData.code));
		}
		else
		{
			throw new NotebookDoesntExists();
		}
	}

	public NotebookData GetByHash(string hash)
	{
		if(Path.Exists(Path.Join(m_directoryPath, hash)))
		{
			NotebookData data = new NotebookData();
			data.hash = hash;
			data.code = File.ReadAllText(Path.Join(m_directoryPath, hash));
			return data;
		}

		throw new NotebookDoesntExists();
	}

	private string m_directoryPath;
}
