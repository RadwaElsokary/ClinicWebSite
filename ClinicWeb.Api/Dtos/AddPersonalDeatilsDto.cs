using ClinicWeb.Domain.Enums;

namespace ClinicWeb.Api.Dtos
{
    public class AddPersonalDeatilsDto
    {
        public IFormFile? Image { set; get; }
        public string? FullName { set; get; }
        public string? Code { set; get; }
        public string? PhoneNumber { set; get; }
        public string? Email { set; get; }
        public int? Age { set; get; }
        public string? Address { set; get; }
        public DateTime? FirstVist { set; get; }
        public string? DrName { set; get; }
        public string? Branch { set; get; }
        public Gender? Gender { set; get; }
        public List<string>? Diagnoses { set; get; }
        public string? Notes { set; get; }
        public string? State { set; get; }
       
    }
}
