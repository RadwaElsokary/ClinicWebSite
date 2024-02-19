using ClinicWeb.Domain.Enums;

namespace ClinicWeb.Api.Dtos
{
    public class UpdateEmployeeDto
    {
        public string? FirstName { set; get; }
        public string? SecondName { set; get; }
        public string? PhoneNumber { set; get; }
        public DateTime? StartAt { set; get; }
        public double? Salary { set; get; }
        public double? Bonus { set; get; }
        public double? Detuct { set; get; }
        
    }
}
