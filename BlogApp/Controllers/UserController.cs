using BlogApp.Context;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace BlogApp.Controllers
{
	public class UserController : Controller
	{

		private readonly DataContext _dataContext;
		public UserController(DataContext dataContext)
		{
			_dataContext = dataContext;

		}

		public IActionResult Login()
		{

			if (User.Identity!.IsAuthenticated)
			{
				return RedirectToAction("Index", "Post");

			}

			return View();

		}
		public IActionResult Register()
		{

			return View();

		}
		[HttpPost]
		public async  Task<IActionResult> Register(RegisterViewModel model)
		{

			if (ModelState.IsValid)
			{
			    
				var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName || x.Email == model.Email);
				if (user == null) 
				{
					_dataContext.Users.Add(new User
					{
						UserName = model.UserName,
						Email = model.Email,
						Password = model.Password,
						Image="avatar.jpg"
					}) ;
					await _dataContext.SaveChangesAsync();
				}
				else 
				{
					ModelState.AddModelError("", "Username yada Email Kullanımda");
				}



				return RedirectToAction("Login");
			}




			return View(model);

		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login");

		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var isUser = await _dataContext.Users.FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);
				if (isUser != null)
				{
					var UserClaims = new List<Claim>();

					UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()));
					UserClaims.Add(new Claim(ClaimTypes.Name, isUser.UserName ?? ""));
					UserClaims.Add(new Claim(ClaimTypes.GivenName, isUser.Name ?? ""));
					UserClaims.Add(new Claim(ClaimTypes.UserData, isUser.Image ?? ""));


					if (isUser.Email == "info@hdrcelikel.com")
					{
						UserClaims.Add(new Claim(ClaimTypes.Role, "admin"));
					}

					var claimsIdentity = new ClaimsIdentity(UserClaims, CookieAuthenticationDefaults.AuthenticationScheme);

					var authProperties = new AuthenticationProperties
					{
						IsPersistent = true,
					};


					await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

					await HttpContext.SignInAsync(
						CookieAuthenticationDefaults.AuthenticationScheme,
						new ClaimsPrincipal(claimsIdentity),
						authProperties
						);

					return RedirectToAction("Index", "Post");
				}
				else
				{
					ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış");
				}
			}



			return View(model);

		}





	}
}
