using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentServer.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AuthController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public  IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel{ReturnUrl=returnUrl });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid && !signInManager.IsSignedIn(User))
            {
               var result = await signInManager.PasswordSignInAsync(vm.UserName, vm.Password, false, false);
                if (result.Succeeded)
                {
                    return Redirect(vm.ReturnUrl);
                }
            }


            return View(vm.ReturnUrl);
        }
        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser(vm.UserName);
                var result = await userManager.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);

                    return Redirect(vm.ReturnUrl);
                }
            }


            return View(vm.ReturnUrl);
        }

    }
}