using ClinicWeb.Api.Dtos;
using ClinicWeb.Domain.Models;
using ClinicWeb.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicWeb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        public SettingController (IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManage, RoleManager<IdentityRole> roleManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManage;
            this.roleManager = roleManager; 
        }

        [HttpPost]
        [Route("AddService")]
        public async  Task<IActionResult> AddService([FromForm] ServicesDto model)
        {
            var service = new Service
            {
                ServiceName = model.Name,
                Price = model.Price
            };
            var result = await unitOfWork.Repository<Service>().Add(service);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Service Added Successfully " });
            }
            return BadRequest(new { message = "Service Not Added" });
        }

        [HttpGet]
        [Route("GetAllServices")]
        public IActionResult GetAllServices()
        {

            var services = unitOfWork.Repository<Service>().GetAll();

            if (services.Any())
            {

                var serviceList = services.Select(service => new
                {
                    Id = service.Id,
                    ServiceName = service.ServiceName,
                    Price = service.Price

                });
                return Ok(serviceList);
            }
            return BadRequest(new { message = "Services Not Found" });

        }

        [HttpPost]
        [Route("AddBranch")]
        public async Task<IActionResult> AddBranch( string Branch)
        {
            var branch = new Branch
            {
                Name = Branch
                
            };
            var result = await unitOfWork.Repository<Branch>().Add(branch);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Branch Added Successfully " });
            }
            return BadRequest(new { message = "Branch Not Added" });
        }

        [HttpGet]
        [Route("GetAllBranchs")]
        public IActionResult GetAllBranchs()
        {

            var branchs = unitOfWork.Repository<Branch>().GetAll();

            if (branchs.Any())
            {

                var branchList = branchs.Select(service => new
                {
                    Id = service.Id,
                    BranchName = service.Name,
                   

                });
                return Ok(branchList);
            }
            return BadRequest(new { message = "Branchs Not Found" });

        }

        [HttpPost]
        [Route("AddDiagnosis")]
        public async Task<IActionResult> AddDiagnosis(string Diagnosis)
        {
            var diagnosis = new Diagnosis
            {
                Name = Diagnosis

            };
            var result = await unitOfWork.Repository<Diagnosis>().Add(diagnosis);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Diagnosis Added Successfully " });
            }
            return BadRequest(new { message = "Diagnosis Not Added" });
        }

        [HttpGet]
        [Route("GetAllDiagnosis")]
        public IActionResult GetAllDiagnosis()
        {

            var diagnosis = unitOfWork.Repository<Diagnosis>().GetAll();

            if (diagnosis.Any())
            {

                var diagnosisList = diagnosis.Select(diagnose => new
                {
                    Id = diagnose.Id,
                    BranchName = diagnose.Name,


                });
                return Ok(diagnosisList);
            }
            return BadRequest(new { message = "Diagnosis Not Found" });

        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser([FromForm] AddUserDto model)
        {

            var existingUserByEmail = await userManager.FindByEmailAsync(model.Email);
            if (existingUserByEmail != null || await userManager.FindByNameAsync(model.Email) != null)
            {
                return Ok(new { message = "Email is already taken" });
            }

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                Job = model.Job
            };

           
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return StatusCode(200, new { message = "You Added User Successfully", UserId = user.Id });
            }
            else
            {
                return StatusCode(400, new { message = "An error occur" });

            }
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {

            var users = unitOfWork.Repository<ApplicationUser>().GetAll();

            if (users.Any())
            {

                var userList = users.Select(user => new
                {
                    Id = user.Id,
                    Name = user.FullName,


                });
                return Ok(userList);
            }
            return BadRequest(new { message = "Users Not Found" });

        }

        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            var user = userManager.FindByIdAsync(UserId).Result;
            if (user == null)
                return BadRequest(new { message = "User Not Found" });

            var result = await userManager.DeleteAsync(user);
            if (result != null)
            {
                await unitOfWork.Complete();
                return StatusCode(200, new { message = "User Deleted Successfully" });
            }
            else
            {
                return StatusCode(400, new { message = "An error occur" });
            }
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(string role)
        {
            var existingRole = await roleManager.FindByNameAsync(role);
            if (existingRole != null)
            {
                return BadRequest("Role already exists");
            }

            var newRole = new IdentityRole(role);
            var result = await roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                return Ok(new { message = "Role added successfully" });
            }
            else
            {
                return BadRequest(new { message = "Role Not Added" });
            }
        }

        [HttpPost]
        [Route("AddRoleToUser")]
        public async Task<IActionResult> AddRoleToUser( string UserId, string RoleName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByIdAsync(UserId);
            if (user is null || !await roleManager.RoleExistsAsync(RoleName))
                return BadRequest(new { message = "Invalid User Or Role" });

            if (await userManager.IsInRoleAsync(user, RoleName))
                return BadRequest(new { message = "User Already Assigned To This Role" });

            var result = await userManager.AddToRoleAsync(user, RoleName);
            if (!result.Succeeded)
                return BadRequest(new { message = "An error Occur" });
            return Ok(new { message = "Role Added To this User Successfully" });
        }

        [HttpPost]
        [Route("DeleteRoleToUser")]
        public async Task<IActionResult> DeleteRoleFromUser(string UserId, string RoleName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByIdAsync(UserId);
            if (user is null || !await roleManager.RoleExistsAsync(RoleName))
                return BadRequest(new { message = "Invalid User Or Role" });

            var result = await userManager.IsInRoleAsync(user, RoleName);
            if (!result)
                return BadRequest(new { message = "User Not Assigned To This Role" });

            var result1 = await userManager.RemoveFromRoleAsync(user, RoleName);
            if (!result1.Succeeded)
                return BadRequest(new { message = "An error Occur" });
            return Ok(new { message = "Role Deleted From This User Successfully" });
        }


    }
}
