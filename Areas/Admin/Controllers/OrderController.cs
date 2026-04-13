using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopComputer.Models;
using ShopComputer.Repository;

namespace ShopComputer.Areas.Admin.Controllers
{
	[Area("Admin")]
	
    public class OrderController : Controller
	{
		private readonly DataContext _dataContext;
		public OrderController(DataContext context)
		{
			_dataContext = context;
		}
		
		public async Task<ActionResult> Index()
		{
			return View(await _dataContext.Orders.OrderByDescending(p => p.Id).ToListAsync());
		}
		
		public async Task<ActionResult> ViewOrder( string ordercode)
		{
			var DetailsOrder = await _dataContext.OrderDetail.Include(od => od.Product).Where(od => od.OrderCode == ordercode).ToListAsync();
			return View(DetailsOrder);
		}
		[HttpPost]
		[Route("UpdateOrder")]
        public async Task<ActionResult> UpdateOrder(string ordercode,int status)
		{
			var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);
			if(order == null)
			{
				return NotFound();
			}

			order.Status = status;

			try
			{
				await _dataContext.SaveChangesAsync();
				return Ok(new { success = true, message = "order status update successfully" });
			}
			catch(Exception ex)
			{
				return StatusCode(500, "looi");
			}
			
		}
		public async Task<IActionResult> Delete(string ordercode)
		{
			var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);
			if (order == null)
			{
				return NotFound();
			}
			_dataContext.Orders.Remove(order);
			await _dataContext.SaveChangesAsync();
			TempData["success"] = "đơn hàng thành công";

			return RedirectToAction("Index");
		}


	}
}
