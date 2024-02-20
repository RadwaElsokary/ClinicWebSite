using ClinicWeb.Api.Dtos;
using ClinicWeb.Domain.Enums;
using ClinicWeb.Domain.Models;
using ClinicWeb.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment hostingEnvironment;

        public EmployeeController(IUnitOfWork unitOfWork , IWebHostEnvironment hostingEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.hostingEnvironment = hostingEnvironment;
        }


        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromForm] AddEmployeeDto model)
        {

            var employee = new Employee
            {
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                Address = model.Address,
                StartAt = model.StartAt,
                Salary = model.Salary,
                CreationDate = DateTime.UtcNow.ToLocalTime(),
                DateBirth = model.DateBirth,
                Gender = model.Gender,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Age = model.Age

            };

            var result = await unitOfWork.Repository<Employee>().Add(employee);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Employee Added Successfully" });
            }
            return BadRequest(new { messag = "Employee Not Added" });

        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public IActionResult GetAllEmployees()
        {

            var employees = unitOfWork.Repository<Employee>().GetAll();

            if (employees.Any())
            {

                var employeeList = employees.Select(employee => new
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    Phone = employee.PhoneNumber,
                    WorkAt = employee.StartAt,
                    Salary = employee.Salary,
                    Bonus = employee.Bonus,
                    Deduct = employee.Detuct,

                });
                return Ok(employeeList);
            }
            return BadRequest(new { message = "Employees Not Found" });

        }

        [HttpGet]
        [Route("GetEmployee")]
        public IActionResult GetEmployee(int EmployeeId)
        {

            var employee = unitOfWork.Repository<Employee>().GetById(EmployeeId).Result;
            if (employee == null)
                return BadRequest(new { message = "Employee Not Found" });



            var response = new
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                Phone = employee.PhoneNumber,
                WorkAt = employee.StartAt,
                Salary = employee.Salary,
                Bonus = employee.Bonus,
                Deduct = employee.Detuct,

            };

                return Ok(response);

        }

        [HttpPut]
        [Route("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromForm] UpdateEmployeeDto model, int EmployeeId)
        {
            var employee = await unitOfWork.Repository<Employee>().GetById(EmployeeId);
            if (employee == null)
                return BadRequest(new { message = "Employee Not Found" });

            employee.FirstName = model.FirstName;
            employee.SecondName = model.SecondName;
            employee.StartAt = (DateTime)model.StartAt;
            employee.PhoneNumber = model.PhoneNumber;
            employee.Salary = (double)model.Salary;
            employee.Bonus = (double)model.Bonus;
            employee.Detuct = (double)model.Detuct;
            

            var result = await unitOfWork.Repository<Employee>().Update(employee);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Employee Updated Successfully" });
            }
            return BadRequest(new { messag = "Employee Not Updated" });

        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(int EmployeeId)
        {
            var employee = await unitOfWork.Repository<Employee>().GetById(EmployeeId);
            if (employee == null)
                return BadRequest(new { message = "Employee Not Found" });

            var result = await unitOfWork.Repository<Employee>().Delete(employee);
            if (result)
            {
                await unitOfWork.Complete();
                return StatusCode(200, new { message = "Employee Deleted Successfully" });
            }
            else
            {
                return StatusCode(400, new { message = "An error occur" });
            }
        }

        [HttpPost]
        [Route("AddRequestForLeave")]
        public async Task<IActionResult> AddRequestForLeave([FromForm] RequestDto model)
        {

            var request = new RequestForLeave
            {
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Reason = model.Reason,
                

            };

            var result = await unitOfWork.Repository<RequestForLeave>().Add(request);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Request Added Successfully" });
            }
            return BadRequest(new { messag = "Request Not Added" });

        }


        [HttpPost]
        [Route("AddAttendance")]
        public async Task<IActionResult> AddAttendance([FromForm] AttendanceDto model)
        {

            var attendance = new Attendance
            {
                FirstName = model.FirstName, 
                LastName = model.LastName,
                Date = model.Date,
                Time = model.Time,
                State = model.State
               
            };

            var result = await unitOfWork.Repository<Attendance>().Add(attendance);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Attendance Added Successfully" });
            }
            return BadRequest(new { messag = "Attendance Not Added" });

        }


        [HttpGet]
        [Route("GetAllAttendance")]
        public IActionResult GetAllAttendance()
        {

            var attendances = unitOfWork.Repository<Attendance>().GetAll();

            if (attendances.Any())
            {

                var attanceList = attendances.Select(attendance => new
                {
                    Id = attendance.Id,
                    FirstName = attendance.FirstName,
                    LastName = attendance.LastName,
                    Date = attendance.Date,
                    Time = attendance.Time,
                   State = attendance.State

                });
                return Ok(attanceList);
            }
            return BadRequest(new { message = "Attendance Not Found" });

        }

        [HttpPost]
        [Route("AddReport")]
        public async Task<IActionResult> AddReport([FromForm] ReportDto model)
        {

            var report = new Report
            {
                Date = model.Date,
                Day = model.Day,
                Branch = model.Branch
            };

            var result = await unitOfWork.Repository<Report>().Add(report);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Report Added Successfully" });
            }
            return BadRequest(new { messag = "Report Not Added" });

        }

        [HttpGet]
        [Route("GetAllReports")]
        public IActionResult GetAllReports()
        {

            var reports = unitOfWork.Repository<Report>().GetAll();

            if (reports.Any())
            {

                var reportList = reports.Select(report => new
                {
                    Id = report.Id,
                    Date = report.Date,
                    Day = report.Day,
                    Branch = report.Branch

                });
                return Ok(reportList);
            }
            return BadRequest(new { message = "Report Not Found" });

        }

        [HttpGet]
        [Route("GetAllEmployeesCount")]
        public IActionResult GetAllEmployeesCount()
        {
            var count = unitOfWork.Repository<Employee>().GetAll().Count();
            return Ok(count);
        }

        [HttpGet]
        [Route("GetAllEmployeesCountMale")]
        public IActionResult GetAllEmployeesCountMale()
        {
            var count = unitOfWork.Repository<Employee>().GetAll().Where(s => s.Gender == Gender.Male).Count();
            return Ok(count);
        }

        [HttpGet]
        [Route("GetAllEmployeesCountFemale")]
        public IActionResult GetAllEmployeesCountFemale()
        {
            var count = unitOfWork.Repository<Employee>().GetAll().Where(s=>s.Gender == Gender.Female).Count();
            return Ok(count);
        }

        [HttpGet]
        [Route("GetAllEmployeesCountForMonth")]
        public IActionResult GetAllEmployeesCountForMonth()
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var count = unitOfWork.Repository<Employee>().GetAll()
                .Count(employee => employee.CreationDate >= thirtyDaysAgo);

            return Ok(count);
        }

        [HttpGet]
        [Route("GetAllEmployeesCountForMonthMale")]
        public IActionResult GetAllEmployeesCountForMonthMale()
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var count = unitOfWork.Repository<Employee>().GetAll()
                .Where(a=>a.Gender == Gender.Male)
                .Count(employee => employee.CreationDate >= thirtyDaysAgo);

            return Ok(count);
        }

        [HttpGet]
        [Route("GetAllEmployeesCountForMonthFeMale")]
        public IActionResult GetAllEmployeesCountForMonthFeMale()
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var count = unitOfWork.Repository<Employee>().GetAll()
                .Where(a => a.Gender == Gender.Female)
                .Count(employee => employee.CreationDate >= thirtyDaysAgo);

            return Ok(count);
        }

    }
}
