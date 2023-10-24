using Microsoft.AspNetCore.Mvc;

using Playground.Notebook;

namespace Playground.Controllers;

[ApiController]
[Route("/api/")]
public class APIController : Controller
{
	public struct HashStruct
	{
		public string hash{get;set;}
	}

	public APIController()
	{
		files = new FilesNotebook("files/");
	}

	[HttpPost("create")]
	public IActionResult Create(HashStruct notebook)
	{
		var notebookData = files.Create(notebook.hash);
		var resp = new HttpResponseMessage();
		HashStruct hashStruct = new HashStruct();
		hashStruct.hash = notebookData.hash;

		return Ok(Json(hashStruct));
	}

	[HttpGet("{hash}")]
	public IActionResult GetByHash(string hash)
	{
		try
		{
			return Ok(files.GetByHash(hash).code);
		}
		catch(NotebookDoesntExists)
		{
			return NotFound();

		}
	}

	[HttpGet("embed/{hash}")]
	public void GetEmbeddedByHash([FromQuery] string hash)
	{

	}

	private FilesNotebook files;
}
