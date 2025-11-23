using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp_identity.Data;
using WebApp_identity.IRepositories;
using WebApp_identity.Models;
using IEmailSender = Microsoft.AspNetCore.Identity.UI.Services.IEmailSender;

namespace WebApp_identity.Controllers;

public class UserController:Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserController(UserManager<ApplicationUser> userManager,IEmailSender emailSender,SignInManager<ApplicationUser> signInManager)
    {
        
        _userManager = userManager;
        _emailSender = emailSender;
        _signInManager = signInManager;
    }
    // [Authorize(Policy = "RequireAdminRole")]
    
    [HttpGet]
    public IActionResult Register()
    {
        var vm = new UserRegisterVM();

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterVM vm)
    {
        var user = vm.Adapt<ApplicationUser>();
        user.UserName = vm.Email;
        var result = await _userManager.CreateAsync(user, vm.Password);
        if (result.Succeeded)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(Confirm),"User",new{token=token,id=user.Id},Request.Scheme);
            var htmlMessage = $"""

                                                                   <html>
                                                                   <body style='font-family: Arial;'>
                                                                       <h2>Welcome to Our Website!</h2>
                                                                       <p>Please confirm your email by clicking the button below:</p>

                                                                       <a href='{link}' 
                                                                          style='display: inline-block; 
                                                                                 padding: 10px 20px; 
                                                                                 background-color: #4CAF50; 
                                                                                 color: white; 
                                                                                 text-decoration: none;
                                                                                 border-radius: 5px;
                                                                                 margin-top: 10px;'>
                                                                           Confirm Email
                                                                       </a>

                                                                       <p style='margin-top:20px;'>If you didn’t request this, just ignore this email.</p>
                                                                   </body>
                                                                   </html>
                               """;

            await _emailSender.SendEmailAsync(vm.Email, "Confirm your email",htmlMessage);
            TempData["SuccessMessage"] = "يرجى تاكيد الايميل!";
            return RedirectToAction(nameof(Login));
            
        }
        
        
        
        
        
        return RedirectToAction(nameof(Register));
    }
    [HttpGet]
    public async Task<IActionResult> Confirm(string token, string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();
        var result = await _userManager.ConfirmEmailAsync(user, token);

        TempData  ["SuccessMessage"] = "تم تاكيد الايميل";
        return RedirectToAction(nameof(Login));
    }
    
    
    
    [HttpGet]
    public IActionResult Login()
    {
        var vm = new LoginVM();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);
        
        var log = await _userManager.FindByEmailAsync(vm.UserName) ?? await _userManager.FindByNameAsync(vm.UserName);
        if (log == null)
        {
            TempData["ErrorMessage"] = "Not Found";
            return View(vm);
        }

        // await _userManager.CheckPasswordAsync(log, vm.Password);
        
        
        var Don = await _signInManager.PasswordSignInAsync(log,vm.Password,lockoutOnFailure:true,isPersistent:vm.RemeberMe);

        if (!Don.Succeeded)
        {
            TempData["ErrorMessage"] = "Password InCorrect";
            return View(vm);
            
        }
        
        
        
        
        
        
        TempData  ["SuccessMessage"] = "Login Successful";
        return RedirectToAction("Index", "Home");
        
        
        // await _userManager.FindByIdAsync(vm.UserName);
        // await _userManager.GetLoginsAsync();
        // await _userManager.GetLoginsAsync();




    }
    
    
    
    
}


