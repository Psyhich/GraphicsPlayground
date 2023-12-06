using System.Security.Cryptography;
using System.Text;

namespace Playground.Models;

public struct ProjectData
{
	public Dictionary<string, string> files{ get; set; } = new Dictionary<string, string>();
	public string name { get; set; } = "";
	public string? description { get; set; } = "";
    public string? hash{ get; set; } = "";
	public string? thumbnail { get; set; } = null;

	public ProjectData()
	{
		this.files = new Dictionary<string, string>();
	}

	public ProjectData(Dictionary<string, string> files)
	{
		this.files = files;
		CalculateHash();
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
