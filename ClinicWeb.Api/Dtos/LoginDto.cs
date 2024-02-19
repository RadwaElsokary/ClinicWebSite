using System.ComponentModel.DataAnnotations;

namespace ClinicWeb.Api.Dtos
{
    public class LoginDto
    {
        public string EmailOrUserName { set; get; }

        [DataType(DataType.Password)]
        public string Password { set; get; }
    }
}
