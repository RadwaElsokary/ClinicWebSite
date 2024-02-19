using ClinicWeb.Api.Services.IServices;
using ClinicWeb.Domain.Models;
using ClinicWeb.Repository;

namespace ClinicWeb.Api.Services.Services
{
    public class PatientServices : IPatientServices
    {
        private readonly ApplicationDbContext context;
        public PatientServices(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Visit> GetVisitByPatient (int PatientId)
        {
            var visit = context.Visits.Where(a => a.PatientId == PatientId).ToList();
            return visit.ToList();
        }
    }
}
