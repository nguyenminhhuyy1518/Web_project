using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopComputer.Areas.Admin.Repository;
using ShopComputer.Models;
using ShopComputer.Models.ViewModel;

namespace ShopComputer.Controllers
{
	

	public class AccountController : Controller
	{
		private UserManager<AppUserModel> _userManager;
		private SignInManager<AppUserModel> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(IEmailSender emailSender,SignInManager<AppUserModel> signInManager,UserManager<AppUserModel> userManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
            _emailSender = emailSender;
        }
		public IActionResult Login( string returnurl)
		{
			return View( new LoginViewModel {  ReturnUrl = returnurl});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel loginvm)
		{
			if (ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginvm.UserName, loginvm.PassWord, false, false);
				if (result.Succeeded)
				{
					return Redirect(loginvm.ReturnUrl ?? "/");
				}
				ModelState.AddModelError("", "sai mat khau hoat username");
			}
			return View(loginvm);
		}


		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create( UserModel user)
		{
			if (ModelState.IsValid)
			{
				AppUserModel newuser = new AppUserModel { UserName=user.UserName,Email = user.Email};
				IdentityResult result = await _userManager.CreateAsync(newuser, user.PassWord);
				if (result.Succeeded)
				{
					TempData["success"] = "tao user thanh cong";
					return Redirect("/account/login");
				}
				foreach(IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return View(user);
		}

		public async Task<IActionResult> Logout(string returnUrl ="/")
		{
			await _signInManager.SignOutAsync();
			return Redirect(returnUrl);
		}

	}
}
