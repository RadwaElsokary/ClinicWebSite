using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicWeb.Domain.Models
{
    public class DiagnosisPatient
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public Patient Patient { set; get; }
        public int PatientId { set; get; }
    }
}
