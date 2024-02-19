using ClinicWeb.Domain.Models;

namespace ClinicWeb.Api.Services.IServices
{
    public interface IPatientServices
    {
        List<Visit> GetVisitByPatient(int PatientId);
    }
}
