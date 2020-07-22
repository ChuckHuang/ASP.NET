using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Core.Models.Request;
using MovieStore.Core.ServiceInterfaces;

namespace MovieStore.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(UserRegisterRequestModel userRegisterRequestModel)
        {
            if (ModelState.IsValid)
            {
                //now call the service
                var createdUser = await _userService.RegisterUser(userRegisterRequestModel);
                return RedirectToAction("Login"); //redirect to an action
            }
            //we take this object from view
            return View();

        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel loginRequest)
        {
            //Http is stateless, each and every request is independent of each other
            //Cookie is one way of storing information on browser, localstorge and sessionsstorage
            //cookies, if there are any cookies present then those cookies will be automatically sent to the server
            //if the cookie is expired or not and valid ot not, then it will go to the database
            //Cookies is one way of state management, client-side
            if (ModelState.IsValid)
            {
                //call service layer to validate user
                var user = await _userService.ValidateUser(loginRequest.Email, loginRequest.Password);
                //we want to show First Name, Last Name on header(navigation)
                //never put sensitive information in the cookie
                //Create Claims based on your application needa
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login");
                }
                var claims = new List<Claim> 
                {
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                };
                //we need to create an Identity Object to hold those claims
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //finally we are going to create a cookie that will be attached to the Http Response
                //HttpContext is probably the most important class in ASP.NET taht holds all the information regarding the HTTP
                //Request/response
                //server will ask the borwer to create the cookie
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                // manually creating cookie
                //HttpContext.Response.Cookies.Append("userLanguage", "English");

                //Once ASP.NET creates Authentication Cookies, it will check for cookie in the HttpRequest 
                //and see if the cookie is expired and it will decrypt the information present in the cookie
                //to check whether User is Authenticated and will also get claims from cookies
                return LocalRedirect("~/");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return LocalRedirect("~/");
        }
    }
}
