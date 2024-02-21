using ClinicWeb.Api.Dtos;
using ClinicWeb.Domain.Enums;
using ClinicWeb.Domain.Models;
using ClinicWeb.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ClinicWeb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        public PatientController (IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }

        private string ProcessUploadFile(IFormFile photo)
        {
            string uniqueFileName = null;
            if (photo != null)
            {
                string uploadFile = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string filePath = Path.Combine(uploadFile, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        [HttpPost]
        [Route("AddPatient")]
        public async Task<IActionResult> AddPatient([FromForm] AddPatientDto model)
        {
 
            var patient = new Patient
            {
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,

            };

            var result = await unitOfWork.Repository<Patient>().Add(patient);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Patient Added Successfully" });
            }
            return BadRequest(new { messag = "Patient Not Added" });    

        }

        [HttpPut]
        [Route("ChangeState")]
        public async Task<IActionResult> AddPersonalDeatilsPatient(string state, int PatientId)
        {            
            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });

           
            patient.State = state;

            var result = await unitOfWork.Repository<Patient>().Update(patient);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "State Changed Successfully" });
            }
            return BadRequest(new { messag = "State Not Changed" });

        }

        [HttpPut]
        [Route("AddPersonalDeatilsPatient")]
        public async Task<IActionResult> AddPersonalDeatilsPatient([FromForm] AddPersonalDeatilsDto model, int PatientId)
        {
            var codeExist = await unitOfWork.Repository<Patient>().AnyAsync(p => p.Code == model.Code && p.Id != PatientId );
            if (codeExist && model.Code != null)
                return BadRequest(new { message = "Exist Code" });
            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);

            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });

            string uniqueFileName = ProcessUploadFile(model.Image);

            patient.FullName = model.FullName;
            patient.PhoneNumber = model.PhoneNumber;
            patient.Email = model.Email;
            patient.Age = model.Age;
            patient.Gender = model.Gender;
            patient.PhotoPath = uniqueFileName;
            patient.Address = model.Address;
            patient.Code = model.Code;
            patient.Branch = model.Branch;
            patient.Diagnoses = model.Diagnoses;
            patient.DrName = model.DrName;
            patient.FirstVist = model.FirstVist;
            patient.Notes = model.Notes;
            patient.State = model.State;

            

            var result = await unitOfWork.Repository<Patient>().Update(patient);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Patient Updated Successfully" });
            }
            return BadRequest(new { messag = "Patient Not Added" });

        }

        [HttpDelete]
        [Route("DeletePatient")]
        public async Task<IActionResult> DeletePatient(int PatientId)
        {
            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });

            var result = await unitOfWork.Repository<Patient>().Delete(patient);
            if (result)
            {
                await unitOfWork.Complete();
                return StatusCode(200, new { message = "Patient Deleted Successfully" });
            }
            else
            {
                return StatusCode(400, new { message = "An error occur" });
            }
        }


        [HttpPost]
        [Route("AddSession")]
        public async Task<IActionResult> AddSession([FromForm] AddSessionDto model , int ServiceId , int PatientId)
        {
            var service = await unitOfWork.Repository<Service>().GetById(ServiceId);
            if (service == null)
                return BadRequest(new { message = "Service Not Exist" });

            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });


            var session = new Session
            {
                ServiceId = service.Id,
                PatientId  = PatientId,
                NumberSessions = model.NumberSessions,
                TotalPrice = model.TotalPrice,
                Status = model.Status
                
            };

            var result = await unitOfWork.Repository<Session>().Add(session);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Session Added Successfully" });
            }
            return BadRequest(new { messag = "Session Not Added" });

        }

        [HttpPost]
        [Route("AddVisit")]
        public async Task<IActionResult> AddVisit([FromForm] VisitDto model, int PatientId)
        {
            
            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });


            var visit = new Visit
            {
                PatientId = PatientId,
                DrName = model.DrName,
                Nurse = model.Nurse,
                SessionNote = model.SessionNote,
                Date = model.Date,
                TotalPrice = model.TotalPrice,
                PaidPrice = model.PaidPrice,
                RemainingPrice = model.RemainingPrice,
                Visa = model.Visa,
                Cash = model.Cash,
                TotalSessions = model.TotalSessions

            };

            var result = await unitOfWork.Repository<Visit>().Add(visit);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Visit Added Successfully" });
            }
            return BadRequest(new { messag = "Visit Not Added" });

        }

        [HttpGet]
        [Route("GetAllPatients")]
        public IActionResult GetAllPatients()
        {

            var patients =  unitOfWork.Repository<Patient>().GetAll();

            if (patients.Any())
            {

                var patientList = patients.Select(patient => new
                {
                    Id = patient.Id,
                    Name = patient.FullName,
                    Phone = patient.PhoneNumber

                });
                return Ok(patientList);
            }
            return BadRequest(new { message = "Patients Not Found" });

        }


        [HttpGet]
        [Route("GePersonalDetails")]
        public async Task<IActionResult> GePersonalDetails(int PatientId)
        {

            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });

            
                return Ok(patient);
            

        }


        [HttpGet]
        [Route("GetAllSessionPatient")]
        public async Task<IActionResult> GetAllSessionPatient(int PatientId)
        {
            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });

            var result = unitOfWork.Repository<Session>().GetAll().Where(v => v.PatientId == PatientId).ToList();

            if (result.Any())
            {
                var partPaid = result.Any(a => a.Status == Status.PartPaid);
                var unpaid = result.Any(a => a.Status == Status.UnPaid);
                var paid = result.Any(a => a.Status == Status.Paid);

                Status paymentStatus;
                if (paid && !partPaid && !unpaid)
                    paymentStatus = Status.Paid;
                else if (!paid && !partPaid && unpaid)
                    paymentStatus = Status.UnPaid;
                else
                    paymentStatus = Status.PartPaid;

                var sessions = result.Select(session => new
                {
                    Id = session.Id,
                    ServiceName = unitOfWork.Repository<Service>().GetById((int)session.ServiceId).Result.ServiceName,
                    Price = unitOfWork.Repository<Service>().GetById((int)session.ServiceId).Result.Price,
                    NoOfSessions = session.NumberSessions,
                });

                double totalPrice = (double)result.Where(a=>a.Status != Status.Paid).Sum(session => session.TotalPrice);

                var response = new
                {
                    SessionList = sessions,
                    PayState = paymentStatus.ToString(),
                    TotalPrice = totalPrice
                };

                return Ok(response);
            }

            return BadRequest(new { message = "Sessions Not Found" });
        }

        [HttpGet]
        [Route("GetAllVisitPatient")]
        public async Task<IActionResult> GetAllVisitPatient(int PatientId)
        {

            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });

            var result = unitOfWork.Repository<Visit>().GetAll().Where(v=>v.PatientId == PatientId).ToList();


            if (result.Any())
            {
                
                var visits = result.Select(visit => new 
                {
                    Id = visit.Id,
                    Dr = visit.DrName,
                    Nurse= visit.Nurse,
                    VistDate = patient.FirstVist,
                    TotalSessions = visit.TotalSessions,
                    Notes = visit.SessionNote,
                    TotalMoney = visit.TotalPrice,
                    Paid = visit.PaidPrice,
                    Remaining = visit.RemainingPrice,

                });
                return Ok(visits);
            }
            return BadRequest(new { message = "Visits Not Found" });

        }

        [HttpGet]
        [Route("GetTotalPatients")]
        public IActionResult TotalPatiens()
        {
            var count = unitOfWork.Repository<Patient>().GetAll().Count();
            return Ok(count);
        }

        [HttpGet]
        [Route("GetTotalAppointments")]
        public IActionResult GetTotalAppointments()
        {
            var count = unitOfWork.Repository<Visit>().GetAll().Count();
            return Ok(count);
        }

        [HttpGet]
        [Route("GetTotalPatientsUnder18")]
        public IActionResult GetTotalPatientsUnder18()
        {
            var count = unitOfWork.Repository<Patient>().GetAll().Where(patient=>patient.Age < 18).Count();
            return Ok(count);
        }

        [HttpGet]
        [Route("GetTotalPatientsUnder30")]
        public IActionResult GetTotalPatientsUnder30()
        {
            var count = unitOfWork.Repository<Patient>().GetAll().Where(patient => patient.Age < 30).Count();
            return Ok(count);
        }

        [HttpGet]
        [Route("GetTotalPatientsUnder50")]
        public IActionResult GetTotalPatientsUnder50()
        {
            var count = unitOfWork.Repository<Patient>().GetAll().Where(patient => patient.Age < 50).Count();
            return Ok(count);
        }

        [HttpGet]
        [Route("GetTotalPatientsAbove50")]
        public IActionResult GetTotalPatientsAbove50()
        {
            var count = unitOfWork.Repository<Patient>().GetAll().Where(patient => patient.Age > 50).Count();
            return Ok(count);
        }

        [HttpGet]
        [Route("GetAllPayment")]
        public IActionResult GetAllPayment()
        {
            var payments = unitOfWork.Repository<Visit>().GetAll().ToList();

            var paymentResponse = payments.Select(payment => new
            {
                ImagePatient = unitOfWork.Repository<Patient>().GetById((int)payment.PatientId).Result.PhotoPath,
                NamePatient = unitOfWork.Repository<Patient>().GetById((int)payment.PatientId).Result.FullName,
                Code = unitOfWork.Repository<Patient>().GetById((int)payment.PatientId).Result.Code,
                Amount = payment.PaidPrice,
                Date = payment.Date,
                PaymentStatus = payment.RemainingPrice == null ? Status.Paid.ToString() : Status.PartPaid.ToString(),
                PaymentMethod = payment.Visa != null && payment.Cash != null ? "Visa and Cash" :
                                payment.Visa != null ? "Visa" :
                                payment.Cash != null ? "Cash" : null
            });

            return Ok(paymentResponse);
        }

        [HttpGet]
        [Route("GetTotalIncome")]
        public IActionResult GetTotalIncome()
        {
            var visits = unitOfWork.Repository<Visit>().GetAll().Sum(a => a.PaidPrice);
            return Ok(visits);
        }

        

    }

}
