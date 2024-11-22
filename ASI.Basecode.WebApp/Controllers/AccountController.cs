using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Authentication;
using ASI.Basecode.WebApp.Models;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AccountController : ControllerBase<AccountController>
    {
        private readonly SessionManager _sessionManager;
        private readonly SignInManager _signInManager;
        private readonly TokenValidationParametersFactory _tokenValidationParametersFactory;
        private readonly TokenProviderOptionsFactory _tokenProviderOptionsFactory;
        private readonly IConfiguration _appConfiguration;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="localizer">The localizer.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="tokenValidationParametersFactory">The token validation parameters factory.</param>
        /// <param name="tokenProviderOptionsFactory">The token provider options factory.</param>
        public AccountController(
                            SignInManager signInManager,
                            IHttpContextAccessor httpContextAccessor,
                            ILoggerFactory loggerFactory,
                            IConfiguration configuration,
                            IMapper mapper,
                            IUserService userService,
                            IEmailService emailService,
                            TokenValidationParametersFactory tokenValidationParametersFactory,
                            TokenProviderOptionsFactory tokenProviderOptionsFactory) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            this._sessionManager = new SessionManager(this._session);
            this._signInManager = signInManager;
            this._tokenProviderOptionsFactory = tokenProviderOptionsFactory;
            this._tokenValidationParametersFactory = tokenValidationParametersFactory;
            this._appConfiguration = configuration;
            this._userService = userService;
            this._emailService = emailService;
        }

        /// <summary>
        /// Login Method
        /// </summary>
        /// <returns>Created response view</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            TempData["returnUrl"] = System.Net.WebUtility.UrlDecode(HttpContext.Request.Query["ReturnUrl"]);
            this._sessionManager.Clear();
            this._session.SetString("SessionId", System.Guid.NewGuid().ToString());
            return this.View();
        }

        /// <summary>
        /// Authenticate user and signs the user in when successful.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns> Created response view </returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            this._session.SetString("HasSession", "Exist");

            User user = null;

            //User user = new() { Id = 0, UserId = "0", Name = "Name", Password = "Password" };

            //await this._signInManager.SignInAsync(user);
            //this._session.SetString("UserName", model.UserId);

            //return RedirectToAction("Index", "Home");

            var loginResult = _userService.AuthenticateUser(model.userIdOrEmail, model.Password, ref user);
            if (loginResult == LoginResult.Success)
            {
                // 認証OK
                TempData["SuccessMessage"] = "Login successful.";
                await this._signInManager.SignInAsync(user);
                this._session.SetString("UserName", user.Name);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // 認証NG
                TempData["ErrorMessage"] = "Incorrect Username, Email or Password";
                return View();
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(UserViewModel userModel)
        {
            try
            {
                // Check if the email already exists
                if (_userService.CheckEmailExists(userModel.Email))
                {
                    TempData["ErrorMessage"] = "This email is already in use.";
                    return View();
                }
                if (_userService.CheckUsernameExists(userModel.UserId))
                {
                    TempData["ErrorMessage"] = "This username is already in use.";
                    return View();
                }

                // Add the new user if the email is unique  
                TempData["SuccessMessage"] = "User created Successfully.";
                _userService.AddUser(userModel);
                return RedirectToAction("Login", "Account");
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again.";
            }

            return View();
        }
        /// <summary>
        /// Sign Out current account and return login view.
        /// </summary>
        /// <returns>Created response view</returns>
        [AllowAnonymous]
        public async Task<IActionResult> SignOutUser()
        {

            TempData["SuccessMessage"] = "Logout successful.";
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear(); // Clear any session data
            await HttpContext.SignOutAsync(); // Ensure cookies are cleared
            return RedirectToAction("Login", "Account");
        }
        public IActionResult SomeAuthenticatedPage()
        {
            Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "-1");
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            var model = new ForgotPasswordViewModel();
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LandingPage()
        {
            return View();
        }
        // ForgotPassword POST method
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                TempData["ErrorMessage"] = "Email address is required.";
                return View(model);
            }

            var resetToken = await _userService.SetPasswordResetTokenAsync(model.Email);
            if (!resetToken)
            {
                TempData["ErrorMessage"] = "The provided email does not exist in our system.";
                return View(model);
            }

            model.EmailIsSent = true; // Set flag if email was sent
            TempData["SuccessMessage"] = "A reset link has been sent.";
            return View(model);
        }

        // ResetPassword GET method
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid token.");
            }

            var user = _userService.GetUserByToken(token);
            if (user == null || user.TokenExpiry < DateTime.UtcNow)
            {
                return BadRequest("Invalid or expired token.");
            }

            var model = new ResetPasswordViewModel
            {
                Email = user.Email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid input.";
                return View(model);
            }

            var user = _userService.GetUserByToken(model.Token);
            if (user == null || user.TokenExpiry < DateTime.UtcNow)
            {
                TempData["ErrorMessage"] = "Invalid or expired reset token.";
                return RedirectToAction("ForgotPassword");
            }

            // Reset the password
            user.Password = PasswordManager.EncryptPassword(model.Password);
            await _userService.ClearResetTokenAsync(user.Id); // Clear the token to prevent reuse

            TempData["SuccessMessage"] = "Password reset successfully!";
            return RedirectToAction("Login");
        }

    }
}