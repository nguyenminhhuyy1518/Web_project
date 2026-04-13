using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopComputer.Models;
using ShopComputer.Repository;

namespace ShopComputer.Areas.Admin.Controllers
{
	[Area("Admin")]
    

    public class CategoryController : Controller
    {
        
        private readonly DataContext _dataContext;
        public CategoryController(DataContext context)
        {
            _dataContext = context;
        }
        
        
        public async Task<ActionResult> Index()
        {
            return View(await _dataContext.Categories.OrderByDescending(p => p.Id).ToListAsync());
        }
        
        
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Create(CategoryModel category)
        {
            
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "san pham da co trong database");
                    return View(category);
                }
               
                _dataContext.Add(category);
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
            return View(category);
        }
       
        public async Task<IActionResult> Delete(int Id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(Id);
            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "xoa thuong hieu thanh cong";

            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> Edit(int Id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(Id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(CategoryModel category)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            var existes_prduct = _dataContext.Categories.Find(category.Id);

            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "san pham da co trong database");
                    return View(existes_prduct);
                }

                existes_prduct.Name = category.Name;
                existes_prduct.Description = category.Description;
                existes_prduct.status = category.status;

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
            return View(category);
        }

    }
}
