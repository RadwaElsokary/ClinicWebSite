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
    }
}
