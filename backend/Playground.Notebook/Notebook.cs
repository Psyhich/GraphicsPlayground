using System.Security.Cryptography;
using System.Text;

namespace Playground.Notebook;

public struct PlaygroundData
{
	public Dictionary<string, string> files{ get; set; }
    public string hash{ get; set; }

	public PlaygroundData()
	{
		this.files = new Dictionary<string, string>{};
		this.hash = "";
	}

	public PlaygroundData(Dictionary<string, string> files)
	{
		this.files = files;
		this.hash = "";
		this.CalculateHash();
	}

	public void CalculateHash()
	{
		StringBuilder allCode = new StringBuilder{};
		allCode.Append(hash);
		foreach (var (key, value) in files)
		{
			allCode.Append(key);
			allCode.Append(value);
		}

		using (HashAlgorithm algorithm = SHA256.Create())
		{
			byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(allCode.ToString());
			byte[] hashBytes = algorithm.ComputeHash(textBytes);

			string hash = BitConverter
				.ToString(hashBytes)
				.Replace("-", String.Empty);

			this.hash = hash;
		}
	}
}

public class PlaygroundDoesntExists : Exception
{}

public interface IPlaygroundRepository
{
	PlaygroundData GetByHash(string hash);

	PlaygroundData Save(PlaygroundData dataToSave);
	void Update(PlaygroundData dataToUpdate);
	void Delete(string hash);
}
