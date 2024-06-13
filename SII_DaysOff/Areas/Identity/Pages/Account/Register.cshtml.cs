﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SII_DaysOff.Areas.Identity.Data;
using SII_DaysOff.Models;

namespace SII_DaysOff.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly DbContextBD _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            DbContextBD context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>

            [Required]
            [Display(Name = "Role")]
            public Guid Role { get; set; }
            
            [Required]
            [Display(Name = "VacationDays")]
            public Guid VacationDays { get; set; }
            
            [Required]
            [Display(Name = "UserVacationDays")]
            public Guid UserVacationDays { get; set; }

            [Required]
            [StringLength(100)]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [StringLength(100)]
            [Display(Name = "Surname")]
            public string Surname { get; set; }

            [Required]
            [Display(Name = "IsActive")]
            public bool IsActive { get; set; }

            [Required]
            [Display(Name = "Manager")]
            public Guid Manager { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            //Console.WriteLine("\n\n\n\tEntraGET Register");
            ReturnUrl = returnUrl;
			//ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
			ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Description");
			ViewData["ManagerId"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
            ViewData["VacationDaysId"] = new SelectList(_context.VacationDays, "Year", "Year");
            /*
             ViewData["ManagerId"] = new SelectList(_context.AspNetUsers.Select(u => new
            {
                FullName = $"{u.Name} - {u.Surname}"
            }), "Id", "FullName");*/
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                var logedInUser = await _userManager.GetUserAsync(User);

                /*user.Profile = Input.Profile;
                user.AvailableDays = int.Parse(Input.AvailableDays);
                user.AcquiredDays = int.Parse(Input.AcquiredDays);
                user.RemainingDays = int.Parse(Input.RemainingDays);*/

                //user.RoleId = Guid.Parse("FA208010-179E-4121-B723-3D449297BBCC");
                user.Name = Input.Name;
                user.Manager = Input.Manager;
                user.Surname = Input.Surname;
                user.UserName = Input.Name + " " + Input.Surname;
                user.IsActive = true;
                user.Manager = Input.Manager;
                user.RegisterDate = DateTime.Now;
                user.RoleId = Input.Role;
                //user.Manager = Guid.Parse("DAB39DCC-4845-438C-B64D-9C3E1E0596B1");
				//user.CreatedBy = logedInUser.Id;
				user.CreationDate = DateTime.Now;
                //user.ModifiedBy = logedInUser.Id;
                user.ModificationDate = DateTime.Now;

                //user.UserVacationDays = userVacationDays;


                /*user.UserVacationDays.Year = "2024";
                user.UserVacationDays.AcquiredDays = 0;
                user.UserVacationDays.AdditionalDays = 0;
                user.UserVacationDays.CreatedBy = logedInUser.Id;
                user.UserVacationDays.CreationDate = DateTime.Now;
                user.UserVacationDays.ModifiedBy = logedInUser.Id;
                user.UserVacationDays.ModificationDate = DateTime.Now;*/

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Confirmar automáticamente el usuario
                    await _userManager.ConfirmEmailAsync(user, "");
                    //await _signInManager.SignInAsync(user, isPersistent: false);

                    //Dias vacaciones
                    /*var userVacationDays = new UserVacationDays();
                    userVacationDays.UserId = user.Id;
                    userVacationDays.Year = "2024";
                    userVacationDays.AcquiredDays = 0;
                    userVacationDays.AdditionalDays = 0;
                    userVacationDays.CreatedBy = logedInUser.Id;
                    userVacationDays.CreationDate = DateTime.Now;
                    userVacationDays.ModifiedBy = logedInUser.Id;
                    userVacationDays.ModificationDate = DateTime.Now;*
                    _context.Add(userVacationDays);*/
                    await _context.SaveChangesAsync();

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        Console.WriteLine("register confirmation false");
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect("~/Home/Main");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }


        private ApplicationUser CreateUser()
        {
            try
            {
                var user = new ApplicationUser();
                user.Id = Guid.NewGuid(); // Generar un nuevo GUID
                return user;
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
