using ClinicWeb.Domain.Models;

namespace ClinicWeb.Api.Services.IServices
{
    public interface IPatientServices
    {
        string GetServiceName(int Id);
    }
}
