using INS364.DigitalNews.Data;
using INS364.DigitalNews.Models;
using INS364.DigitalNews.ViewModels.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace INS364.DigitalNews.Controllers
{
    // Controller used in the authentication and registration of users at the database in the API.
    public class AuthController : Controller
    {
        private IUserRepository _userData { get; }

        public AuthController(IUserRepository userData)
        {
            _userData = userData;
        }

        /// <summary>
        /// Loads the login page.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
            }

            ViewBag.Valid = null;
            return View("Login");
        }

        /// <summary>
        /// Loads the user registration page.
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            ViewBag.Success = null;
            return View("Signup");
        }

        /// <summary>
        /// Authenticates the user based on the values inserted in the login page.
        /// </summary>
        /// <param name="loginInfo">The login input from the login page.</param>
        /// <returns></returns>
        public async Task<IActionResult> AuthenticateUser(LoginViewModel loginInfo, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                loginInfo.Password = loginInfo.Password.Trim();
                loginInfo.Username = loginInfo.Username.Trim();
                string hashed_password = HashifyPassword(loginInfo.Password); 
                loginInfo.Password = hashed_password;
                UserModel user = await _userData.GetUserInfo(loginInfo);
                ViewBag.Valid = user != null;

                if (ViewBag.Valid == true)
                {
                    var identity = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                    }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);
                    await Request.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = true,
                        AllowRefresh = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(10)
                    });

                    // In case the user logs in while requesting content from a page with denied access.
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");                    
                }
            }

            return View("Login");
        }

        /// <summary>
        /// Adds a new user in the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> RegisterUser(SignupViewModel signupInfo)
        {
            ViewBag.Success = (false, true);
            if (ModelState.IsValid)
            {
                bool noDuplicatesExists = await _userData.GetAnyUserDuplicates(signupInfo.Username);
                if (noDuplicatesExists)
                {
                    signupInfo.Password = HashifyPassword(signupInfo.Password);
                    ViewBag.Success = (true, await _userData.RegisterUserAsync(signupInfo));
                }

                else
                {
                    ViewBag.Success = (false, false);
                }
            }

            ModelState.Clear();
            return View("Signup");
        }

        [Authorize]
        public async Task<IActionResult> SignoutUser()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Implements the SHA256 hashing algorithm to 
        /// generate a hashed string of characters.
        /// </summary>
        /// <param name="password">The input string, which is the user's password in this context</param>
        /// <returns></returns>
        private string HashifyPassword(string password)
        {
            string hashed_password;
            using (HashAlgorithm algorithm = SHA256.Create())
            {
                Encoding encoding = Encoding.UTF8;
                StringBuilder sb = new StringBuilder();
                byte[] result = algorithm.ComputeHash(encoding.GetBytes(password));
                foreach (byte _byte in result)
                {
                    sb.Append(_byte.ToString("X2"));
                }

                hashed_password = sb.ToString();
            }

            return hashed_password;
        }
    }
}