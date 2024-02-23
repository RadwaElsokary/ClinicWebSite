using ClinicWeb.Api.Services.IServices;
using ClinicWeb.Domain.Models;
using ClinicWeb.Repository;

namespace ClinicWeb.Api.Services.Services
{
    public class PatientServices : IPatientServices
    {
        private readonly ApplicationDbContext context;
        private string services;

        public PatientServices(ApplicationDbContext context)
        {
            this.context = context;
        }

        public string GetServiceName(int Id)
        {
            var service = context.Services.FirstOrDefault(a=>a.Id == Id).ServiceName.ToString();
            return services;
        }
    }
}
