using Microsoft.AspNetCore.Mvc;

using Playground.Data;
using Playground.Models;

public class EditorController : Controller
{
	public EditorController(IProjectRepository projectRepository)
	{
		m_projectRepository = projectRepository;
	}

	[HttpGet("/editor")]
	public IActionResult Index()
	{
		return View("Editor", new ProjectData());
	}

	[HttpGet("/editor/{hash}")]
	public IActionResult Edit(string hash)
	{
		try
		{
			var projectData = m_projectRepository.GetByHash(hash);
			return View("Editor", projectData);
		}
		catch (ProjectDoesntExists)
		{
			return NotFound();
		}
		catch (ProjectHasInvalidData ex)
		{
			return BadRequest(ex.Message);
		}
	}

	private readonly IProjectRepository m_projectRepository;
}

