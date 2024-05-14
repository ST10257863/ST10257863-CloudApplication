using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudApplication.Controllers
{
	public class ProductController : Controller
	{
		public productTable prodtbl = new productTable();

		[HttpPost]
		public ActionResult MyWork(productTable products)
		{
			var newProduct = prodtbl.insertProduct(products);
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public ActionResult MyWork()
		{
			return View(prodtbl);
		}
	}
}
