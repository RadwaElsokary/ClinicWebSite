using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicWeb.Domain.Models
{
    public class Visit
    {
        public int Id { set; get; }
        public string DrName { set; get; }
        public string Nurse { set; get; }
        public DateTime Date { set; get; }
        public int TotalSessions { set; get; }
        public string SessionNote { set; get; }
        public double TotalPrice { set; get; }
        public double? RemainingPrice { set; get; }
        public double PaidPrice { set; get; }
        public double? Cash { set; get; }
        public double? Visa { set; get; }
        public Patient Patient { set; get; }
        public int PatientId {set; get;}

    }
}
