using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicWeb.Domain.Models
{
    public class Service
    {
        public int Id { set; get; }
        public string ServiceName { set; get; }
        public double Price { set; get; }
    }
}
