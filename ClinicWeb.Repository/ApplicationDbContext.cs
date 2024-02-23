using ClinicWeb.Domain.EntityMapper;
using ClinicWeb.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ClinicWeb.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PatientMap());

        }

        public virtual DbSet<Patient> Patients { set; get; }
        public virtual DbSet<Session> Sessions { set; get; }
        public virtual DbSet<Service> Services { set; get; }
        public virtual DbSet<Visit> Visits { set; get; }
        public virtual DbSet<Employee> Employees { set; get; }
        public virtual DbSet<Diagnosis> Diagnoses { set; get; }
        public virtual DbSet<DiagnosisPatient> DiagnosisPatients { set; get; }
        public virtual DbSet<Branch> Branches { set; get; }
        public virtual DbSet<Attendance> Attendances { set; get; }
        public virtual DbSet<RequestForLeave> RequestForLeaves { set; get; }
        public virtual DbSet<Report> Reports { set; get; }
        public virtual DbSet<Doctor> Doctors { set; get; }
        public virtual DbSet<State> States { set; get; }


    }
}
