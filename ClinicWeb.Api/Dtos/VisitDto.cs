using ClinicWeb.Domain.Models;

namespace ClinicWeb.Api.Dtos
{
    public class VisitDto
    {
        public string? DrName { set; get; }
        public string? Nurse { set; get; }
        public DateTime? Date { set; get; }
        public string? SessionNote { set; get; }
        public double? TotalPrice { set; get; }
        public double? RemainingPrice { set; get; }
        public double PaidPrice { set; get; }
        public double? Cash { set; get; }
        public double? Visa { set; get; }
       
    }
}
