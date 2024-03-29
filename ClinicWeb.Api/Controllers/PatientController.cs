﻿using ClinicWeb.Api.Dtos;
using ClinicWeb.Domain.Enums;
using ClinicWeb.Domain.Models;
using ClinicWeb.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
namespace ClinicWeb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        public PatientController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
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
        public async Task<IActionResult> AddPatient([FromForm] AddPersonalDeatilsDto model)
        {

            var codeExist = await unitOfWork.Repository<Patient>().AnyAsync(p => p.Code == model.Code);
            if (codeExist && model.Code != null)
                return BadRequest(new { message = "Exist Code!" });

            string uniqueFileName = ProcessUploadFile(model.Image);

            var patient = new Patient
            {
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Age = model.Age,
                Gender = model.Gender,
                PhotoPath = uniqueFileName,
                Address = model.Address,
                Code = model.Code,
                Branch = model.Branch,
                Diagnoses = model.Diagnoses.Select(diagnosis => new DiagnosisPatient { Name = diagnosis }).ToList(),
                DrName = model.DrName,
                FirstVist = model.FirstVist,
                Notes = model.Notes,
                State = model.State,
                Discount = 0
            };

            var result = await unitOfWork.Repository<Patient>().Add(patient);
            if (result)
            {


                await unitOfWork.Complete();
                return Ok(new { message = "Patient Added Successfully" });
            }
            return BadRequest(new { message = "An error occurred while adding the patient" });
        }

        [HttpPut]
        [Route("UpdatePatient")]
        public async Task<IActionResult> UpdatePatient([FromForm] AddPersonalDeatilsDto model, int PatientId)
        {
            var codeExist = await unitOfWork.Repository<Patient>().AnyAsync(p => p.Code == model.Code && p.Id != PatientId);
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
            patient.DrName = model.DrName;
            patient.FirstVist = model.FirstVist;
            patient.Notes = model.Notes;
            patient.State = model.State;

            var existingDiagnoses =  unitOfWork.Repository<DiagnosisPatient>().GetAll().Where(d => d.PatientId == PatientId).ToList();
            foreach (var diagnosis in existingDiagnoses)
            {
                await unitOfWork.Repository<DiagnosisPatient>().Delete(diagnosis);
                await unitOfWork.Complete();
            }

            patient.Diagnoses = model.Diagnoses.Select(diagnosis => new DiagnosisPatient { Name = diagnosis }).ToList();

            var result = await unitOfWork.Repository<Patient>().Update(patient);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Patient Updated Successfully" });
            }
            return BadRequest(new { message = "Patient Not Updated" });
        }

        [HttpDelete]
        [Route("DeletePatient")]
        public async Task<IActionResult> DeletePatient(int PatientId)
        {
            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });

            var visits = unitOfWork.Repository<Visit>().GetAll().Where(v => v.PatientId == PatientId).ToList();
            foreach (var visit in visits)
            {
                await unitOfWork.Repository<Visit>().Delete(visit);
            }

            var result = await unitOfWork.Repository<Patient>().Delete(patient);
            if (result)
            {
                await unitOfWork.Complete();
                return StatusCode(200, new { message = "Patient Deleted Successfully" });
            }
            else
            {
                return StatusCode(400, new { message = "An error occurred" });
            }
        }
        [HttpPost]
        [Route("AddSession")]
        public async Task<IActionResult> AddSession([FromForm] AddSessionDto model, int ServiceId, int PatientId)
        {
            var service = await unitOfWork.Repository<Service>().GetById(ServiceId);
            if (service == null)
                return BadRequest(new { message = "Service Not Exist" });

            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });

            var session = new Session
            {
                ServiceId = ServiceId,
                PatientId = PatientId,
                NumberSessions = model.NumberSessions,
                TotalPrice = model.TotalPrice,

            };
            //patient.TotalPrice = patient.TotalPrice + session.TotalPrice;
            var result = await unitOfWork.Repository<Session>().Add(session);
            if (result)
            {
                await unitOfWork.Repository<Patient>().Update(patient);
                await unitOfWork.Complete();
                return Ok(new { message = "Session Added Successfully" });
            }
            return BadRequest(new { messag = "Session Not Added" });
        }

        [HttpGet]
        [Route("GetSession")]
        public async Task<IActionResult> GetSession(int SessionId)
        {
            var session = await unitOfWork.Repository<Session>().GetById(SessionId);
            if (session == null)
                return BadRequest(new { message = "Session Not Exist" });

            var service = await unitOfWork.Repository<Service>().GetById(session.Id);
            if (service == null)
                return BadRequest(new { message = "Service Not Exist" });

            return Ok(new { SessionService = service.ServiceName, ServicePrice = service.Price, NOSessions = session.NumberSessions , TotalPrice = session.TotalPrice });

        }


        [HttpPut]
        [Route("UpdateSession")]
        public async Task<IActionResult> UpdateSession([FromForm] AddSessionDto model, int SessionId, int ServiceId)
        {
            var session = await unitOfWork.Repository<Session>().GetById(SessionId);
            if (session == null)
                return BadRequest(new { message = "Session Not Exist" });

            var service = await unitOfWork.Repository<Service>().GetById(ServiceId);
            if (service == null)
                return BadRequest(new { message = "Service Not Exist" });

            var patient = await unitOfWork.Repository<Patient>().GetById(session.PatientId);


            session.TotalPrice = model.TotalPrice;
            session.NumberSessions = model.NumberSessions;
            session.ServiceId = ServiceId;

            var result = await unitOfWork.Repository<Session>().Update(session);

            if (result)
            {

                await unitOfWork.Repository<Patient>().Update(patient);
                await unitOfWork.Complete();
                return Ok(new { message = "Session Updated Successfully" });
            }
            return BadRequest(new { messag = "Session Not Updated" });

        }

        [HttpDelete]
        [Route("DeleteSession")]
        public async Task<IActionResult> DeleteSession(int SessionId)
        {
            var session = await unitOfWork.Repository<Session>().GetById(SessionId);
            if (session == null)
                return BadRequest(new { message = "Session Not Exist" });

            var patient = await unitOfWork.Repository<Patient>().GetById(session.PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });


            var result = await unitOfWork.Repository<Session>().Delete(session);
            if (!result)
                return BadRequest(new { message = "Session Not Deleted" });

            var remainingSessionsExist = unitOfWork.Repository<Session>().GetAll().Any(s => s.PatientId == session.PatientId && s.Id != SessionId);
            if (!remainingSessionsExist)
            {
                patient.Discount = 0;
                await unitOfWork.Repository<Patient>().Update(patient);
            }
            else
            {
                await unitOfWork.Repository<Patient>().Update(patient);
            }

            await unitOfWork.Complete();
            return Ok(new { message = "Session Deleted Successfully" });
        }

        [HttpGet]
        [Route("GetTotalPriceToVisit")]
        public async Task<IActionResult> GetTotalPriceToVisit(int PatientId)
        {
            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });
            var totalPrice = unitOfWork.Repository<Session>().GetAll().Where(a => a.PatientId == PatientId).Select(a => a.TotalPrice).Sum();


            return Ok(new { totalPrice = totalPrice - patient.Discount });
        }

        [HttpGet]
        [Route("GetRemaningPrice")]
        public async Task<IActionResult> GetRemaningPrice(int PatientId)
        {
            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });

            var result = unitOfWork.Repository<Visit>().GetAll().Where(v => v.PatientId == PatientId).ToList();

            var paidMoney = result.Sum(a => a.PaidPrice);
            var totalPrice = unitOfWork.Repository<Session>().GetAll().Where(a => a.PatientId == PatientId).Select(a => a.TotalPrice).Sum();
            var price = totalPrice - patient.Discount;

            var remaning = price - result.Sum(session => session.PaidPrice);

            return Ok(new { remaning = remaning });
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
        [Route("GetVisit")]
        public async Task<IActionResult> GetVisit(int VisitId)
        {

            var visit = await unitOfWork.Repository<Visit>().GetById(VisitId);
            if (visit == null)
                return BadRequest(new { message = "Patient Not Found" });

            return Ok(visit);


        }


        [HttpPut]
        [Route("UpdateVisit")]
        public async Task<IActionResult> UpdateVisit([FromForm] VisitDto model, int VisitId)
        {

            var visit = await unitOfWork.Repository<Visit>().GetById(VisitId);
            if (visit == null)
                return BadRequest(new { message = "Patient Not Found" });


            visit.DrName = model.DrName;
            visit.Nurse = model.Nurse;
            visit.SessionNote = model.SessionNote;
            visit.Date = model.Date;
            visit.TotalPrice = model.TotalPrice;
            visit.PaidPrice = model.PaidPrice;
            visit.RemainingPrice = model.RemainingPrice;
            visit.Visa = model.Visa;
            visit.Cash = model.Cash;



            var result = await unitOfWork.Repository<Visit>().Update(visit);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Visit Updated Successfully" });
            }
            return BadRequest(new { messag = "Visit Not Updated" });

        }

        [HttpDelete]
        [Route("DeleteVisit")]
        public async Task<IActionResult> DeleteVisit(int VisitId)
        {

            var visit = await unitOfWork.Repository<Visit>().GetById(VisitId);
            if (visit == null)
                return BadRequest(new { message = "Patient Not Found" });

            var result = await unitOfWork.Repository<Visit>().Delete(visit);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Visit Deleted Successfully" });
            }
            return BadRequest(new { messag = "Visit Not Deleted" });

        }

        [HttpGet]
        [Route("GetAllPatients")]
        public IActionResult GetAllPatients()
        {
            var patients = unitOfWork.Repository<Patient>().GetAll().ToList();

            if (patients.Any())
            {
                var patientList = new List<object>();

                foreach (var patient in patients)
                {
                    var lastVisitDate = unitOfWork.Repository<Visit>()
                        .GetAll()
                        .Where(v => v.PatientId == patient.Id)
                        .OrderByDescending(v => v.Date)
                        .Select(v => v.Date)
                        .FirstOrDefault();

                    var patientSessions = unitOfWork.Repository<Session>()
                        .GetAll()
                        .Where(a => a.PatientId == patient.Id)
                        .Select(session => new
                        {
                            Id = session.Id,
                            ServiceId = session.ServiceId,
                            NoOfSessions = session.NumberSessions,
                        })
                        .ToList();

                    var formattedSessions = new List<object>();

                    foreach (var session in patientSessions)
                    {
                        var serviceName = unitOfWork.Repository<Service>()
                            .GetById(session.ServiceId.Value)?.Result.ServiceName;


                        formattedSessions.Add(new
                        {
                            Id = session.Id,
                            ServiceName = serviceName,
                            NoOfSessions = session.NoOfSessions,
                        });
                    }

                    var formattedPatient = new
                    {
                        Id = patient.Id,
                        Name = patient.FullName,
                        Phone = patient.PhoneNumber,
                        LastVisitDate = lastVisitDate,
                        Sessions = formattedSessions,
                    };

                    patientList.Add(formattedPatient);
                }

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

            var resonse = new
            {
                Id = patient.Id,
                FullName = patient.FullName,
                Code = patient.Code,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                Age = patient.Age,
                Address = patient.Address,
                FirstVist = patient.FirstVist,
                DrName = patient.DrName,
                Branch = patient.Branch,
                Gender = patient.Gender,
                Diagnoses = unitOfWork.Repository<DiagnosisPatient>().GetAll().Where(a => a.PatientId == PatientId).Select(a => a.Name),
                Notes = patient.Notes,
                State = patient.Notes,
                PhotoPath = patient.PhotoPath
            };


            return Ok(resonse);


        }


        [HttpGet]
        [Route("GetAllSessionPatient")]
        public async Task<IActionResult> GetAllSessionPatient(int PatientId)
        {
            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });

            var result = unitOfWork.Repository<Session>().GetAll().Where(v => v.PatientId == PatientId).ToList();

            if (result.Count() > 0)
            {
                var sessions = result.Select(session => new
                {
                    Id = session.Id,
                    Service = unitOfWork.Repository<Service>().GetById(session.ServiceId.Value).Result,
                    NoOfSessions = session.NumberSessions,
                }).Select(session => new
                {
                    Id = session.Id,
                    ServiceName = session.Service != null ? session.Service.ServiceName : null,
                    ServicePrice = session.Service != null ? (double?)session.Service.Price : null,
                    TotalPrice = session.Service.Price * session.NoOfSessions,
                    NoOfSessions = session.NoOfSessions,
                });

                return Ok(sessions);
            }

            return BadRequest(new { message = "Sessions Not Found" });
        }


        [HttpGet]
        [Route("GetTotalPricesSession")]
        public async Task<IActionResult> GetTotalPriceSession(int PatientId)
        {
            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });

            var result = unitOfWork.Repository<Session>().GetAll().Where(v => v.PatientId == PatientId).ToList();

            double totalPrice = (double)result.Sum(session => session.TotalPrice);

                var response = new
                {
                    TotalPriceBeforeDiscound = totalPrice,
                    Discound = patient.Discount,
                    TotalPriceAfterDiscound = totalPrice - patient.Discount

                };


                return Ok(response);
            
        }



        [HttpPut]
        [Route("MakeDiscoundSession")]
        public async Task<IActionResult> MakeDiscoundSession(int Discound , int PatientId)
        {
            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });

            patient.Discount = patient.Discount + Discound;

            var result = await unitOfWork.Repository<Patient>().Update(patient);
            if(result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Discound Applayed Successfully" });
            }

            return BadRequest(new { message = "Discound Not Applayed" });
        }

        [HttpPut]
        [Route("UpdateDiscoundSession")]
        public async Task<IActionResult> UpdateDiscoundSession(int Discound, int PatientId)
        {
            var patient = await unitOfWork.Repository<Patient>().GetById(PatientId);
            if (patient == null)
                return BadRequest(new { message = "Patient Not Found" });

            patient.Discount = Discound;

            var result = await unitOfWork.Repository<Patient>().Update(patient);
            if (result)
            {
                await unitOfWork.Complete();
                return Ok(new { message = "Discound Appalyed Successfully" });
            }

            return BadRequest(new { message = "Discound Appalyed" });
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
        public IActionResult GetTotalPatients()
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
