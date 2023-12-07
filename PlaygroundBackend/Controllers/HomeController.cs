using Microsoft.AspNetCore.Mvc;
using Playground.Data;

namespace Playground.Services;

public class HomeController : Controller
{
	public HomeController(IProjectRepository projectsRepository)
	{
		m_projectsRepository = projectsRepository;
	}

    public ActionResult Index()
    {
		var projects = m_projectsRepository.GetProjectsSince(DateTime.Now.AddMonths(-1));
		if (projects.Count > 0)
		{
			ViewData["featured"] = projects[0]; 
			projects.RemoveAt(0);
		}
		else
		{
			ViewData["featured"] = null;
		}
		ViewData["projects"] = projects;
		return View("~/Views/index.cshtml");
	}

	private readonly IProjectRepository m_projectsRepository;
}
