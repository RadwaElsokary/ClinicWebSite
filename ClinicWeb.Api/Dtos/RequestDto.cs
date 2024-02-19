namespace ClinicWeb.Api.Dtos
{
    public class RequestDto
    {
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string Reason { set; get; }
    }
}
