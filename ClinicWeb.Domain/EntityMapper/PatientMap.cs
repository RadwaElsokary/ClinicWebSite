using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicWeb.Domain.Models;

namespace ClinicWeb.Domain.EntityMapper
{
    public class PatientMap : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasIndex(x => x.Code).IsUnique(true);

        }
    }
}
