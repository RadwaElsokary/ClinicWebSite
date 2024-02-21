using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicWeb.Domain.Models
{
    public class Report
    {
        public int Id { set; get; }
        public string? Date { set; get; }
        public string? Day { set; get; }
        public string? Branch { set; get; }
    }
}
