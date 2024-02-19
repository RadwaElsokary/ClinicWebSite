using ClinicWeb.Domain.Enums;

namespace ClinicWeb.Api.Dtos
{
    public class AttendanceDto
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Date { set; get; }
        public string Time { set; get; }
        public AttendanceState State { set; get; }
    }
}
