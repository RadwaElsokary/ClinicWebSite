using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicWeb.Domain.Models
{
    public class RequestForLeave
    {
        public int Id { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string Reason { set; get; }
    }
}
