using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicWeb.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { set; get; }
        public string? City { set; get; }
        public string? Country { set; get; }
        public string? Address { set; get; }
        public string? State { set; get; }
        public string? Bio { set; get; }
        public string? PhotoPath { set; get; }
        public string? Job { set; get; }
    }
}
