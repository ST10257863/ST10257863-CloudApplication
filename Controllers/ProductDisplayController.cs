using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudApplication.Controllers
{
	public class ProductDisplayController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			var products = ProductDisplayModel.SelectProducts();
			return View(products);
		}
	}
}
