using ClinicWeb.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicWeb.Domain.Models
{
    public class Attendance
    {
        public int Id { set; get; }
        public string? FirstName { set; get; }
        public string? LastName { set; get; }
        public string? Date { set; get; }
        public string? Time { set; get; }
        public AttendanceState? State { set; get; }
    }
}
