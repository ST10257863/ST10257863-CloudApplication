using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudApplication.Controllers
{
	public class ProductController : Controller
	{
		public productModel prodtbl = new productModel();

		[HttpPost]
		public ActionResult MyWorkInsertProduct(productModel products)
		{
			var newProduct = prodtbl.insertProduct(products);
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public ActionResult MyWork()
		{
			return View(prodtbl);
		}

		[HttpGet]//This is for placing orders do not remove
		public IActionResult IndexRetrieveProducts()
		{
			var products = productModel.retrieveProducts();
			return View(products);
		}
	}
}
