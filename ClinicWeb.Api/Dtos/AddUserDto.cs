using System.ComponentModel.DataAnnotations;

namespace ClinicWeb.Api.Dtos
{
    public class AddUserDto
    {
        public string FullName { set; get; }
        public string Email { set; get; }
        public string Job { set; get; }

        [DataType(DataType.Password)]
        public string Password { set; get; }
    }
}
