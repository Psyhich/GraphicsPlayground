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

	public struct CodeStruct
	{
		public string code{get;set;}
	}

	public APIController()
	{
		files = new FilesNotebook("files/");
	}

	[HttpPost("create")]
	public IActionResult Create(CodeStruct notebook)
	{
		var notebookData = files.Create(notebook.code);
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
