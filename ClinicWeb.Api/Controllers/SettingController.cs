using ClinicWeb.Api.Dtos;
using ClinicWeb.Domain.Models;
using ClinicWeb.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public SettingController (IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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
    }
}
