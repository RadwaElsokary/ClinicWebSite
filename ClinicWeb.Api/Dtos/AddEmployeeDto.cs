using ClinicWeb.Domain.Enums;

namespace ClinicWeb.Api.Dtos
{
    public class AddEmployeeDto
    {
        public string? FirstName { set; get; }
        public string? SecondName { set; get; }
        public string? Email { set; get; }
        public string? PhoneNumber { set; get; }
        public DateTime? StartAt { set; get; }
        public double? Salary { set; get; }
        public DateTime? DateBirth { set; get; }
        public Gender? Gender { set; get; }
        public double? Age { set; get; }
        public string? Address { set; get; }
    }
}
