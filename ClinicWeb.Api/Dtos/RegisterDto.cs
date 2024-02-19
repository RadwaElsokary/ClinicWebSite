using System.ComponentModel.DataAnnotations;

namespace ClinicWeb.Api.Dtos
{
    public class RegisterDto
    {
        public string FullName { set; get; }
        public string Email { set; get; }
        public string UserName { set; get; }

        [DataType(DataType.Password)]
        public string Password { set; get; }
    }
}
