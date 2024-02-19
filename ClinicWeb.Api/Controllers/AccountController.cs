using ClinicWeb.Api.Dtos;
using ClinicWeb.Domain;
using ClinicWeb.Domain.Enums;
using ClinicWeb.Domain.Models;
using ClinicWeb.Repository.IRepository;
using ClinicWeb.Repository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using System.IdentityModel.Tokens.Jwt;

namespace ClinicWeb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment hostingEnvironment;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.unitOfWork = unitOfWork;
            this.hostingEnvironment = hostingEnvironment;
        }

        private string ProcessUploadFile(IFormFile Photo)
        {
            string uniqueFileName = null;
            if (Photo != null)
            {
                string uploadFile = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                string filePath = Path.Combine(uploadFile, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


      
        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration([FromForm] RegisterDto model)
        {
            try
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
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return StatusCode(400, new { message = "User Not Added", errors });
                }
            }
            catch (Exception ex)
            {
                // Log the exception for further investigation
                // You can use a logging framework like Serilog or log to a file

                return StatusCode(500, new { message = "An error occurred during user registration" + ex });
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

        [HttpGet]
        [Route("GetProfile")]
        public async Task<IActionResult> GetProfile(string  Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
                return BadRequest(new { message = "User Not Found" });

            var userDto = new
            {
                PhotoPath = user.PhotoPath,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FullName,
                Phone = user.PhoneNumber,
                Bio  = user.Bio,
                City = user.City,
                Country = user.Country,
                State = user.State,


            };
            return Ok(userDto);
        }

        [HttpPut]
        [Route("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDto model, string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
                return BadRequest(new { message = "User Not Found" });


            bool emailExists = await unitOfWork.Repository<ApplicationUser>().AnyAsync(u => u.Email == model.Email && u.Id != Id);

            if (emailExists)
                return BadRequest(new { message = "Email already exists" });

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Bio = model.Bio;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.City = model.City;
            user.Country = model.Country;
            user.UserName = model.UserName;
            user.State = model.State;
            
            
            if (!string.IsNullOrWhiteSpace(model.Email))
                user.Email = model.Email;

            if (model.Photo != null)
            {
                string FilePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.Photo.ToString());
                System.IO.File.Delete(FilePath);
            }
            user.PhotoPath = ProcessUploadFile(model.Photo);

            var result = userManager.UpdateAsync(user);
            if (result == null)
                return BadRequest(new { message = "Profile Can't Updated " });

            return Ok(new { message = "Profile Updated Successfully" });
        }


    }
}
