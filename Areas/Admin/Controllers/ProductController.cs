using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using ShopComputer.Models;
using ShopComputer.Repository;

namespace ShopComputer.Areas.Admin.Controllers
{
	[Area("Admin")]
    
  //  [Authorize(Roles = "Admin")]

    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }
       
		public async Task<ActionResult> Index()
		{
			return View(await _dataContext.Products.OrderByDescending(p=>p.Id).Include(c=>c.Category).Include(b=>b.Brand).ToListAsync());
		}

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Create(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "san pham da co trong database");
                    return View(product);
                }
                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath,"media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string fivePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(fivePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;
                }
                _dataContext.Add(product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "them san pham thanh cong";
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
            return View(product);
        }
        
        public async Task<IActionResult> Edit( int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View(product);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            var existes_prduct = _dataContext.Products.Find(product.Id);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");
                
                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string fivePath = Path.Combine(uploadDir, imageName);
                    string oldfilePath = Path.Combine(uploadDir, existes_prduct.Image);

                    try
                    {
                        if (System.IO.File.Exists(oldfilePath))
                        {
                            System.IO.File.Delete(oldfilePath);
                        }
                    }
                    catch(Exception ex)
                    {
                        ModelState.AddModelError("", " erro loi ko xoa dc anh");
                    }

                    FileStream fs = new FileStream(fivePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    existes_prduct.Image = imageName;
                }

                existes_prduct.Name = product.Name;
                existes_prduct.Description = product.Description;
                existes_prduct.Price = product.Price;
                existes_prduct.CategoryId = product.CategoryId;
                existes_prduct.BrandId = product.BrandId;

                _dataContext.Update(existes_prduct);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Edit san pham thanh cong";
                return RedirectToAction("Index");

                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "san pham da co trong database");
                    return View(existes_prduct);
                }
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
            return View(product);
        }
        
        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            if (!string.Equals(product.Image, "noname.jpg"))
            {
                string upladDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                string oldfileImager = Path.Combine(upladDir, product.Image);
                if (System.IO.File.Exists(oldfileImager))
                {
                    System.IO.File.Delete(oldfileImager);
                }
            }
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            TempData["erro"] = "xoa san pham thanh cong";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddQuantity ( int Id)
        {
            var productbyquantity = await _dataContext.ProductQuantities.Where(pq => pq.ProductId == Id).ToListAsync();
            ViewBag.ProductByQuantity = productbyquantity;
            ViewBag.Id = Id;
            return View();
        }

        [Route("StoreProductQuantity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StoreProductQuantity(ProductQuantityModel productQuantity)
        {
            var product = _dataContext.Products.Find(productQuantity.ProductId);
            if ( product == null)
            {
                return NotFound();
            }
            product.Quantity += productQuantity.Quantity;

            productQuantity.Quantity = productQuantity.Quantity;
            productQuantity.ProductId = productQuantity.ProductId; 
            productQuantity.DataCreated = DateTime.Now;

            _dataContext.Add(productQuantity);
            _dataContext.SaveChangesAsync();
            TempData["success"] = " thêm số lượng sản phẩm thành công";
            return RedirectToAction("AddQuantity", "Product", new { Id = productQuantity.ProductId });
        }
    }
}
