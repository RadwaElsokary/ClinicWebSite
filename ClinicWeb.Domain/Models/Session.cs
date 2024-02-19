using ClinicWeb.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicWeb.Domain.Models
{
    public class Session
    {
        public int Id { set; get; }
        public int ServiceId { set; get; }
        public int NumberSessions { set; get; }
        public Status Status{set; get;}
        public double TotalPrice { set; get; } 
        public Patient Patient { set; get; }
        public int PatientId { set; get; }
    }
}
