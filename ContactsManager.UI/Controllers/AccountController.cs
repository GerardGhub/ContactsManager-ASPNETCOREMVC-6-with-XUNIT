using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using CRUDExample.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
    //[AllowAnonymous]
    //Controller for handling user account related operations
    public class AccountController : Controller
    {
        // Dependencies for managing users, sign-ins, and roles
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        // Constructor to initialize UserManager, SignInManager, and RoleManager
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // Displays the registration form
        [HttpGet]
        [Authorize("NotAuthorized")] // Requires authentication using "NotAuthorized" policy
        public IActionResult Register()
        {
            return View();
        }


        // Handles form submission for user registration
        [HttpPost]
        [Authorize("NotAuthorized")] // Requires authentication using "NotAuthorized" policy
                                     //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            //Check for validation errors
            if (ModelState.IsValid == false)
            {
                // Collect and display validation errors in Viewbag for the view
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(registerDTO);
            }

            // Create ApplicationUser object with registration data
            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone,
                UserName = registerDTO.Email,
                PersonName = registerDTO.PersonName
            };

            // Attempt to create user using UserManager
            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                //Check status of radio button
                //Check the user type selected in registration form
                if (registerDTO.UserType == Core.Enums.UserTypeOptions.Admin)
                {
                    //Create 'Admin' role if not already exists
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    //Add the new user into 'Admin' role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                }
                else
                {
                    //Create 'User' role if not already exists
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.User.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    //Add the new user into 'User' role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                }
                //Sign in the user after successful registration
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Redirect to the index action of PersonsController
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            else
            {
                // if user creation fails, add errors  to ModelState and return to registration view
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }

                return View(registerDTO);
            }

        }





        // Displays the login form
        [HttpGet]
        [Authorize("NotAuthorized")]  // Requires authentication using "NotAuthorized" policy
        public IActionResult Login()
        {
            return View();
        }

        // Handles form submission for user login
        [HttpPost]
        [Authorize("NotAuthorized")] // Requires authentication using "NotAuthorized" policy
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            // Check for validation errors
            if (!ModelState.IsValid)
            {
                // Collect and display validation errors in ViewBag for the view
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(loginDTO);
            }

            // Attempt to sign in user using SignInManager
            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                //Admin
                // If login successful, determine user role and redirect accordingly
                ApplicationUser user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user != null)
                {
                    if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
                    {
                        // Redirect to Admin area if user is an admin
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }

                }

                // Redirect to original ReturnUrl if valid and local
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }

                // Default redirect to PersonsController Index action
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }

            // If login fails, add error to ModelState and return to login view
            ModelState.AddModelError("Login", "Inalid email or password");
            return View(loginDTO);
        }

        // Logs out the currently authenticated user
        [Authorize] // Requires authentication
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();   // Sign out the current user
                                                   // Redirect to PersonsController Index action after logout
            return RedirectToAction(nameof(PersonsController.Index), "Persons");
        }

        // Checks if the provided email is already registered asynchronously
        [AllowAnonymous] // Allows access without authentication
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true); //valid
            }
            else
            {
                return Json(false); //invalid
            }
        }
    }
}
