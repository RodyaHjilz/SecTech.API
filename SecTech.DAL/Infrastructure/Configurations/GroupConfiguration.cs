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
    public class GroupConfiguration : IEntityTypeConfiguration<UGroup>
    {
        public void Configure(EntityTypeBuilder<UGroup> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Users).WithMany(x => x.Groups);
        }
    }
}
