using ClinicWeb.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicWeb.Domain.Models
{
    public class Employee
    {
        public int Id { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }
        public DateTime StartAt { set; get; }
        public double Salary { set; get; }
        public DateTime DateBirth { set; get; }
        public Gender Gender { set; get; }
        public double Age { set; get; }
        public string Address { set; get; }
        public double Bonus { set; get; }
        public double Detuct { set; get; }
        public DateTime CreationDate { set; get; }

    }
}
