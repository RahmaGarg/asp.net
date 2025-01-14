using Atelier_2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Atelier_2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
public async Task<IActionResult> Profile()
{
    var user = await userManager.GetUserAsync(User);

    if (user == null)
    {
        return RedirectToAction("Login", "Account");
    }

    // Obtenir les revendications (claims) de l'utilisateur
    var claims = await userManager.GetClaimsAsync(user);
    var adresseClaim = claims.FirstOrDefault(c => c.Type == "Adresse");

    var model = new ProfileViewModel
    {
        Email = user.Email,
        Adresse = adresseClaim?.Value, // Utiliser la valeur du claim si elle existe
        NumeroTelephone = user.PhoneNumber
    };

    return View(model);
}

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Logout()

        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var claims = await userManager.GetClaimsAsync(user);
            var adresseClaim = claims.FirstOrDefault(c => c.Type == "adresse");

            var model = new ProfileViewModel
            {
                Email = user.Email,
                Adresse = adresseClaim?.Value, // Utiliser la valeur du claim si elle existe
                NumeroTelephone = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileViewModel model)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            user.PhoneNumber = model.NumeroTelephone;

            // Check if Adresse is not null or empty before adding the claim
            if (!string.IsNullOrEmpty(model.Adresse))
            {
                var claims = await userManager.GetClaimsAsync(user);
                var adresseClaim = claims.FirstOrDefault(c => c.Type == "Adresse");

                if (adresseClaim != null)
                {
                    await userManager.RemoveClaimAsync(user, adresseClaim);
                }

                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Adresse", model.Adresse));
            }

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }
    }
}
