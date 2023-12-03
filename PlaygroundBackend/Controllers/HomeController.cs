using Microsoft.AspNetCore.Mvc;

class HomeController : Controller
{
	[Route("")]
    public ActionResult Index()
    {
		Console.WriteLine("Request index");
		return View("~/Views/index.cshtml");
	}
}
