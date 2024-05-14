using Microsoft.AspNetCore.Mvc;

namespace CloudApplication.Controllers
{
	public class TransactionController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
