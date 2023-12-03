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
		return View("~/Views/index.cshtml", m_projectsRepository.GetProjectsSince(DateTime.Now.AddMonths(-1)));
	}

	private readonly IProjectRepository m_projectsRepository;
}
