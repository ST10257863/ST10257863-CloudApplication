using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudApplication.Controllers
{
	public class ProductController : Controller
	{
		public ProductModel prodtbl = new ProductModel();

		[HttpPost]
		public ActionResult MyWorkInsertProduct(ProductModel products)
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
