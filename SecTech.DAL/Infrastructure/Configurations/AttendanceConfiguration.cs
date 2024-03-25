using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecTech.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.DAL.Infrastructure.Configurations
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasOne(x => x.Event)
                .WithMany(x => x.Attendances)
                .HasForeignKey(x => x.EventId)    // Точно ли именно этот ключ?
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Attendances)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
                
        }
    }
}
