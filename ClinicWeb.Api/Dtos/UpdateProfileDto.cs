namespace ClinicWeb.Api.Dtos
{
    public class UpdateProfileDto
    {
        public IFormFile? Photo { set; get; }
        public string? PhoneNumber { set; get; }
        public string? Email { set; get; }
        public string? FullName { set; get; }
        public string? UserName { set; get; }
        public string? City { set; get; }
        public string? Country { set; get; }
        public string? Address { set; get; }
        public string? State { set; get; }
        public string? Bio { set; get; }
    }
}
