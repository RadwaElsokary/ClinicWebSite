using ClinicWeb.Domain.Enums;
using ClinicWeb.Domain.Models;

namespace ClinicWeb.Api.Dtos
{
    public class AddSessionDto
    {
        public int? NumberSessions { set; get; }
        public Status? Status { set; get; }
        public double? TotalPrice { set; get; }
    }
}
