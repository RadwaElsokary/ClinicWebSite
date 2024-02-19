using ClinicWeb.Api.Dtos;
using ClinicWeb.Domain;
using ClinicWeb.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace ClinicWeb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration([FromForm] RegisterDto model)
        {
            var existingUserByEmail = await userManager.FindByEmailAsync(model.Email);
            if (existingUserByEmail != null || await userManager.FindByNameAsync(model.Email) != null)
            {
                return Ok(new { message = "Email is already taken" });
            }


            var existingUsername = await userManager.FindByNameAsync(model.UserName);
            if (existingUsername != null || await userManager.FindByEmailAsync(model.UserName) != null)
            {
                return Ok(new { message = "Username is already taken" });
            }

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.UserName,
                FullName = model.FullName,
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return StatusCode(200, new { message = "You Registered Successfully", UserId = user.Id });
            }
            else
            {
                return StatusCode(400, new { message = "User Not Added" });
            }
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await userManager.FindByNameAsync(model.EmailOrUserName);
            if (user == null)
            {
                user = await userManager.FindByEmailAsync(model.EmailOrUserName);
            }

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    
                    return Ok(new { message = "You Logged Successfully"});
                }
            }

           
            return BadRequest("Invalid username/email or password.");
        }

    }
}
