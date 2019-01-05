using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppIdentity.Models;
using WebAppIdentity.ViewModels;
using System.Threading.Tasks;
using System;
using System.Linq;
using WebAppIdentity.EF;
using Microsoft.AspNetCore.Authorization;

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
        public IActionResult SignUp() => View(new SignUpViewModel());

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            User user = new User()
            {
                UserName = model.Email,
                FullName = model.FullName,
                Email = model.Email,
                Gender = model.Gender,
                BirthDate = model.BirthDate
            };


            IdentityResult res = await userManager.CreateAsync(user, model.Password);
            if (res.Succeeded) {
                var result = await userManager.AddToRoleAsync(user, "user");
                var emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var tokenVerificationUrl = Url.Action(
                    "VerifyEmail", "Account", new { userId = user.Id, token = emailConfirmationToken },
                    protocol: HttpContext.Request.Scheme);

                return View("VerifyEmail", tokenVerificationUrl);
            }
            else {
                foreach (var er in res.Errors) {
                    ModelState.AddModelError("Email", er.Description);
                }
                return View(model);
            }
        }

        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException();

            var emailConfirmationResult = await userManager.ConfirmEmailAsync(user, token);
            if (!emailConfirmationResult.Succeeded)
                return RedirectToAction("Index", "Home");
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult SignIn() => View(new SignInViewModel());

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (!result.Succeeded) {
                ModelState.AddModelError("SignInError", "Invalid login or password.Please try again.");
                return View();
            }
            return RedirectToAction("Index", "Account", new { model.Email });
        }


        public async Task<IActionResult> Index(string email)
        {
            var user = await userManager.FindByNameAsync(email);
            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                ViewBag.Admin = userRoles.Contains("admin") ? true : false;

                return View(user);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            return View(new EditUserViewModel { Id = user.Id, FullName = user.FullName ,BirthDate = user.BirthDate, Gender = user.Gender });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            User user = await userManager.FindByIdAsync(model.Id);
            if (user != null) {
                user.FullName = model.FullName;
                user.BirthDate = model.BirthDate;
                user.Gender = model.Gender;

                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Account", new { user.Email });
                }
                else {
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword(string id, string email) => View(new ChangePasswordViewModel { Id = id, Email = email });

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            User user = await userManager.FindByIdAsync(model.Id);
            if (user != null) {
                IdentityResult result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded) {
                    return RedirectToAction("Index", "Account", new { user.Email });

                }
                else {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError("ChangePasswordError", error.Description);
                    }
                }
            }
            else {
                ModelState.AddModelError(string.Empty, "Not Found");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);
            if (user == null || !(await userManager.IsEmailConfirmedAsync(user))) {
                ModelState.AddModelError("ForgotPasswordError", "Error!");
                return View();
            }

            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { user.Email, code },
                protocol: HttpContext.Request.Scheme);

            return View("ForgotPasswordConfirmation", callbackUrl);
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string code = null)
        {
            return code == null ? View("ForgotPassword") : View(new ResetPasswordViewModel() { Email = email});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);
            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded) {
                ViewData["ResetPassword"] = true;
                return View("SignIn");
            }
            foreach (var error in result.Errors) {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}