using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudApplication.Controllers
{
	public class ProductController : Controller
	{
		public ProductModel prodtbl = new ProductModel();

		[HttpPost]
		public ActionResult MyWorkInsertProduct(ProductModel product)
		{
			int result = prodtbl.InsertProduct(product);
			return RedirectToAction("Index", "Home");
		}
	}
}
