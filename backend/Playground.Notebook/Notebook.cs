using System.Security.Cryptography;
using System.Text;

namespace Playground.Notebook;

public struct NotebookData
{
	public String code;
	public String hash;

	public NotebookData()
	{
		this.code = "";
		this.hash = "";
	}

	public NotebookData(string code)
	{
		this.code = code;
		this.hash = GetHash(code);
	}

	public static string GetHash(string code)
	{
		using (HashAlgorithm algorithm = SHA256.Create())
		{
			byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(code);
			byte[] hashBytes = algorithm.ComputeHash(textBytes);

			string hash = BitConverter
				.ToString(hashBytes)
				.Replace("-", String.Empty);

			return hash;
		}
	}
}

public class NotebookDoesntExists : Exception
{}

public interface INotebookRepository
{
	NotebookData Create(string code);
	NotebookData GetByHash(string hash);
	void Edit(NotebookData editedData);
	void Delete(string hash);
}
