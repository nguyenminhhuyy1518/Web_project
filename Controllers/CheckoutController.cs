using ShopComputer.Areas.Admin.Repository;
using Microsoft.AspNetCore.Mvc;
using ShopComputer.Models;
using ShopComputer.Repository;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ShopComputer.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataContext;
		private readonly IEmailSender _emailSender;

		public CheckoutController(IEmailSender emailSender, DataContext context)
		{
			_dataContext = context;
			_emailSender = emailSender;
		}

		public async Task<IActionResult> Checkout() {
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if(userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}
			else
			{
				var ordercode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel();
				orderItem.OrderCode = ordercode;
				orderItem.UserName = userEmail;
				orderItem.Status = 1;
				orderItem.CreateDate = DateTime.Now;
				_dataContext.Add(orderItem);
				_dataContext.SaveChanges();
				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
				foreach(var cart in cartItems)
				{
					var orderdetails = new OrderDetails();
					orderdetails.UserName = userEmail;
					orderdetails.OrderCode = ordercode;
					orderdetails.ProductId = cart.ProductId;
					orderdetails.Price = cart.Price;
					orderdetails.Quatity = cart.Quantity;

					var product = await _dataContext.Products.Where(p => p.Id == cart.ProductId).FirstAsync();
					product.Quantity -= cart.Quantity;
					product.Sold += cart.Quantity;
					_dataContext.Update(product);

					_dataContext.Add(orderdetails);
					_dataContext.SaveChanges();
				}
				HttpContext.Session.Remove("Cart");
				//send mail oder when success
				var receiver = userEmail;
				var subject = "đặt hàng thành công";
				var message = "đăng hàng thành công, trải nghiệm dịch vụ nhé";

				await _emailSender.SendEmailAsync(receiver, subject, message);

                TempData["success"] = "đơn hàng đã được nhận vui lòng chờ duyệt đơn hàng nhé";
				return RedirectToAction("Index", "Cart");
			}
			return View();
			
		}
	}
}
