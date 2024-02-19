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
    }
}
