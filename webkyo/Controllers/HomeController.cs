using System.Web.Mvc;

namespace webkyo.Controllers
{
    [Authorize]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}
