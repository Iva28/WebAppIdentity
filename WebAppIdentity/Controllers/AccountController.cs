using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppIdentity.Models;
using WebAppIdentity.ViewModels;
using System.Threading.Tasks;

namespace WebAppIdentity.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["UserName"] = User.Identity.Name;
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View(new SignUpViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            User user = new User()
            {
                UserName = model.FullName,
                FullName = model.FullName,
                Email = model.Email,
                EmailConfirmed = false,
                Gender = model.Gender,
                BirthDate = model.BirthDate
            };

            IdentityResult res =await userManager.CreateAsync(user, model.Password);
            if (res.Succeeded)
            {
                await signInManager.SignInAsync(user, true); //куки будут сохраняться при закрытии браузера
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var er in res.Errors)
                {
                    ModelState.AddModelError("Email", er.Description);
                }
                return View(model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}