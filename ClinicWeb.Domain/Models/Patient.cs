using ClinicWeb.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicWeb.Domain.Models
{
    public class Patient
    {
        public int Id { set; get; }
        public string? FullName { set; get; }
        public string? Code { set; get; }
        public string? PhoneNumber { set; get; }
        public string? Email { set; get; }
        public int? Age { set; get; }
        public string? Address { set; get; }
        public DateTime? FirstVist { set; get; }
        public string? DrName { set; get; }
        public string? Branch { set; get; }
        public Gender? Gender { set; get; }
        public string? Diagnoses { set; get; }
        public string? Notes { set; get; }
        public string? State { set; get; } 
        public string? PhotoPath { set; get; }
        public int? TotalPriceSessions { set; get; }
        public List<Session>? Sessions { set; get; }
        public List<Visit>? Visits { set; get; }
    }
}
