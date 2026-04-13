using Microsoft.AspNetCore.Mvc;

namespace ShopComputer.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
