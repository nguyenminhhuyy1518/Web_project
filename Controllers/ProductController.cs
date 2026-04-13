using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopComputer.Models;
using ShopComputer.Models.ViewModel;
using ShopComputer.Repository;

namespace ShopComputer.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
		public ProductController(DataContext context)
		{
			_dataContext = context;
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Detail(int Id)
		{
			if (Id  == null) return RedirectToAction("Index");
			var productsById = _dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();
			if (productsById == null) return RedirectToAction("Index");

			//related product
			var relateProducts = await _dataContext.Products.Include(p=>p.Comments).Where(p => p.CategoryId == productsById.CategoryId && p.Id != productsById.Id).Take(4).ToListAsync();
			ViewBag.RelateProducts = relateProducts;

			var viewmodel = new ProductDetailViewModel
			{
				ProductDetail = productsById,
				//CommentDetail = productsById.Comments,
			};
			return View(viewmodel);
		}

		public async Task<IActionResult>Search(string searchTerm)
		{
			var product = await _dataContext.Products.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)).ToListAsync();
			ViewBag.keyworld = searchTerm;
			return View(product);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CommentProduct(CommentModel comment)
		{
			
			if (ModelState.IsValid){

				var commentEmtity = new CommentModel
				{
					ProductId = comment.ProductId,
					Name= comment.Name,
					Email= comment.Email,
					Comment=comment.Comment,
					Star=comment.Star
				};

				_dataContext.Comments.Add(commentEmtity);
				await _dataContext.SaveChangesAsync();

				TempData["success"] = " thêm đánh giá thành công";

				return Redirect(Request.Headers["Referer"]);
			}
			else
			{
				foreach (var key in Request.Form.Keys)
				{
					Console.WriteLine($"{key}: {Request.Form[key]}");
				}
				TempData["erro"] = "model loi";
				List<string> errors = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach (var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);
					}
				}
				string erromessage = string.Join("\n", errors);

				return RedirectToAction("Detail", new { id = comment.ProductId });
			}
			return Redirect(Request.Headers["Referer"]);
		}
	}
}
