using ClinicWeb.Api.Dtos;
using ClinicWeb.Domain.Models;
using ClinicWeb.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
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
        [Route("GetService")]
        public async Task<IActionResult> GetService( int ServiceId)
        {
            var service = await unitOfWork.Repository<Service>().GetById(ServiceId);
            if (service == null)
                return BadRequest(new { message = "Service Not Found" });

                return Ok(new { Name  = service.ServiceName , Price = service.Price });
           
        }



        [HttpPut]
        [Route("UpdateService")]
        public async Task<IActionResult> UpdateService([FromForm] ServicesDto model, int ServiceId)
        {
            var service = await unitOfWork.Repository<Service>().GetById(ServiceId);
            service.ServiceName = model.Name;
            service.Price = model.Price;
           
            var result = await unitOfWork.Repository<Service>().Update(service);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Service Updated Successfully " });
            }
            return BadRequest(new { message = "Service Not Updated" });
        }

        [HttpDelete]
        [Route("DeleteService")]
        public async Task<IActionResult> DeleteService(int ServiceId)
        {
            var service = await unitOfWork.Repository<Service>().GetById(ServiceId);

            var result = await unitOfWork.Repository<Service>().Delete(service);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Service Deleted Successfully " });
            }
            return BadRequest(new { message = "Service Not Deleted" });
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
        [Route("GetBranch")]
        public async Task<IActionResult> GetBranch(int Id)
        {
            var branch = await unitOfWork.Repository<Branch>().GetById(Id);
            if (branch == null)
                return BadRequest(new { message = "Branch Not Found" });

            return Ok(new { Name = branch.Name});

        }



        [HttpPut]
        [Route("UpdateBranch")]
        public async Task<IActionResult> UpdateBranch( int BranchID,string? Branch)
        {
            var branch = await unitOfWork.Repository<Branch>().GetById(BranchID);
            branch.Name = Branch;
           
            var result = await unitOfWork.Repository<Branch>().Update(branch);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Branch Updated Successfully " });
            }
            return BadRequest(new { message = "Branch Not Update" });
        }
        [HttpDelete]
        [Route("DeleteBranch")]
        public async Task<IActionResult> DeleteBranch(int BranchId)
        {
            var branch = await unitOfWork.Repository<Branch>().GetById(BranchId);

            var result = await unitOfWork.Repository<Branch>().Delete(branch);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Branch Deleted Successfully " });
            }
            return BadRequest(new { message = "Branch Not Deleted" });
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
        [Route("GetDiagnosis")]
        public async Task<IActionResult> GetDiagnosis(int Id)
        {
            var diagnosis = await unitOfWork.Repository<Diagnosis>().GetById(Id);
            if (diagnosis == null)
                return BadRequest(new { message = "Diagnosis Not Found" });

            return Ok(new { Name = diagnosis.Name });

        }

        [HttpPut]
        [Route("UpdateDiagnosis")]
        public async Task<IActionResult> UpdateDiagnosis(int DiagnosisId,string Diagnosis)
        {
            var disgnosis = await unitOfWork.Repository<Diagnosis>().GetById(DiagnosisId);
            disgnosis.Name = Diagnosis;

            var result = await unitOfWork.Repository<Diagnosis>().Update(disgnosis);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Diagnosis Updated Successfully " });
            }
            return BadRequest(new { message = "Diagnosis Not Updated" });
        }

        [HttpDelete]
        [Route("DeleteDiagnosis")]
        public async Task<IActionResult> DeleteDiagnosis(int DiagnosisId)
        {
            var diagnosis = await unitOfWork.Repository<Diagnosis>().GetById(DiagnosisId);

            var result = await unitOfWork.Repository<Diagnosis>().Delete(diagnosis);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Branch Deleted Successfully " });
            }
            return BadRequest(new { message = "Branch Not Deleted" });
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
            var users = unitOfWork.Repository<ApplicationUser>().GetAll().ToList();

            var usersList = users.Select(user =>
            {
                var roles = userManager.GetRolesAsync(user);

                return new
                {
                    Id = user.Id,
                    Name = user.FullName,
                    Roles = roles.Result
                };
            });

            return Ok(usersList);
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


        [HttpGet]
        [Route("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var roles = unitOfWork.Repository<IdentityRole>().GetAll();

            if (roles.Any())
            {
                var roleList = roles.Select(role => new
                {
                    Id = role.Id,
                    Name = role.Name
                });

                return Ok(roleList);
            }

            return BadRequest(new { message = "Roles Not Found" });
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


        [HttpPost]
        [Route("AddDoctor")]
        public async Task<IActionResult> AddDoctor(string Doctor)
        {
            var doctor = new Doctor
            {
                Name = Doctor

            };
            var result = await unitOfWork.Repository<Doctor>().Add(doctor);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Doctor Added Successfully " });
            }
            return BadRequest(new { message = "Doctor Not Added" });
        }


        [HttpGet]
        [Route("GetDoctor")]
        public async Task<IActionResult> GetDoctor(int Id)
        {
            var doctor = await unitOfWork.Repository<Doctor>().GetById(Id);
            if (doctor == null)
                return BadRequest(new { message = "Doctors Not Found" });

            return Ok(new { Name = doctor.Name });

        }

        [HttpPut]
        [Route("UpdateDoctor")]
        public async Task<IActionResult> UpdateDoctor(int DrId, string? Doctor)
        {
            var dr = await unitOfWork.Repository<Doctor>().GetById(DrId);
            dr.Name = Doctor;

            var result = await unitOfWork.Repository<Doctor>().Update(dr);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Doctor Updated Successfully " });
            }
            return BadRequest(new { message = "Doctor Not Update" });
        }
        [HttpDelete]
        [Route("DeleteDoctor")]
        public async Task<IActionResult> DeleteDoctor(int DrId)
        {
            var doctor = await unitOfWork.Repository<Doctor>().GetById(DrId);

            var result = await unitOfWork.Repository<Doctor>().Delete(doctor);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Doctor Deleted Successfully " });
            }
            return BadRequest(new { message = "Doctor Not Deleted" });
        }

        [HttpGet]
        [Route("GetAllDoctors")]
        public IActionResult GetAllDoctors()
        {

            var doctors = unitOfWork.Repository<Doctor>().GetAll();

            if (doctors.Any())
            {

                var drList = doctors.Select(dr => new
                {
                    Id = dr.Id,
                    DrName = dr.Name,


                });
                return Ok(drList);
            }
            return BadRequest(new { message = "Doctors Not Found" });

        }


        [HttpPost]
        [Route("AddState")]
        public async Task<IActionResult> AddState(string State)
        {
            var state = new State
            {
                Name = State

            };
            var result = await unitOfWork.Repository<State>().Add(state);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "State Added Successfully " });
            }
            return BadRequest(new { message = "State Not Added" });
        }

        [HttpGet]
        [Route("GetState")]
        public async Task<IActionResult> GetState(int Id)
        {
            var state = await unitOfWork.Repository<State>().GetById(Id);
            if (state  == null)
                return BadRequest(new { message = "Sates Not Found" });

            return Ok(new { Name = state.Name });

        }


        [HttpPut]
        [Route("UpdateState")]
        public async Task<IActionResult> UpdateState(int StateId, string? State)
        {
            var state = await unitOfWork.Repository<State>().GetById(StateId);
            state.Name = State;

            var result = await unitOfWork.Repository<State>().Update(state);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "State Updated Successfully " });
            }
            return BadRequest(new { message = "State Not Update" });
        }
        [HttpDelete]
        [Route("DeleteState")]
        public async Task<IActionResult> DeleteState(int StateId)
        {
            var state = await unitOfWork.Repository<State>().GetById(StateId);

            var result = await unitOfWork.Repository<State>().Delete(state);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "State Deleted Successfully " });
            }
            return BadRequest(new { message = "State Not Deleted" });
        }

        [HttpGet]
        [Route("GetAllStates")]
        public IActionResult GetAllStates()
        {

            var states = unitOfWork.Repository<State>().GetAll();

            if (states.Any())
            {

                var stateList = states.Select(state => new
                {
                    Id = state.Id,
                    StateName = state.Name,


                });
                return Ok(stateList);
            }
            return BadRequest(new { message = "States Not Found" });

        }


    }
}
