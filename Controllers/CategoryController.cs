using Microsoft.AspNetCore.Mvc;
using ShopComputer.Models;
using ShopComputer.Repository;
using Microsoft.EntityFrameworkCore;

namespace ShopComputer.Controllers
{
	public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;
		public CategoryController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index(String Slug = "")
		{
			CategoryModel category = _dataContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();
			if (category == null) return RedirectToAction("Index");
			var productscategory = _dataContext.Products.Where(p => p.CategoryId == category.Id);
			return View(await productscategory.OrderByDescending(P => P.Id).ToListAsync());
		}
		/*public async Task<IActionResult> Index(String Slug="")
		{
			CategoryModel category = _dataContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();
			if (category == null) return RedirectToAction("Index");
			var productsByCategory = _dataContext.Products.Where(p => p.CategoryId == category.Id);
			return View(await productsByCategory.OrderByDescending(P => P.Id).ToListAsync());		
		}*/
	}

}
