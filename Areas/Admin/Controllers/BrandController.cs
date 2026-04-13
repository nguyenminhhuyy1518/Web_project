using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopComputer.Models;
using ShopComputer.Repository;

namespace ShopComputer.Areas.Admin.Controllers
{
    [Area("Admin")]
    

    public class BrandController : Controller
    {
		private readonly DataContext _dataContext;
		public BrandController(DataContext context)
		{
			_dataContext = context;
		}
        
        public async Task<ActionResult> Index()
		{
			return View(await _dataContext.Brands.OrderByDescending(p => p.Id).ToListAsync());
		}
        [HttpGet]
        
        public IActionResult Create()
		{

			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Create(BrandModel brand)
        {

            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "san pham da co trong database");
                    return View(brand);
                }

                _dataContext.Add(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "them danh muc thanh cong";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["erro"] = "model loi";
                List<string> erros = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var erro in value.Errors)
                    {
                        erros.Add(erro.ErrorMessage);
                    }
                }
                string erromessage = string.Join("\n", erros);
                return BadRequest(erromessage);

            }
            return View(brand);
        }
       
        public async Task<IActionResult> Delete(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id);
            _dataContext.Brands.Remove(brand);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "xoa san pham thanh cong";

            return RedirectToAction("Index");
        }
       
        public async Task<IActionResult> Edit(int Id)
        {
            BrandModel category = await _dataContext.Brands.FindAsync(Id);
            ViewBag.Brand = new SelectList(_dataContext.Brands, "Id", "Name");
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Edit(BrandModel brand)
        {
            ViewBag.Brand = new SelectList(_dataContext.Brands, "Id", "Name");
            var existes_prduct = _dataContext.Brands.Find(brand.Id);

            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "san pham da co trong database");
                    return View(existes_prduct);
                }

                existes_prduct.Name = brand.Name;
                existes_prduct.Description = brand.Description;
                existes_prduct.status = brand.status;

                _dataContext.Update(existes_prduct);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Edit san pham thanh cong";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["erro"] = "model loi";
                List<string> erros = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var erro in value.Errors)
                    {
                        erros.Add(erro.ErrorMessage);
                    }
                }
                string erromessage = string.Join("\n", erros);
                return BadRequest(erromessage);

            }
            return View(brand);
        }
    }
}
