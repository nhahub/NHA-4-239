using HouraDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouraDAL.Data.Configurations
{
    public class ServicePostingConfiguration : IEntityTypeConfiguration<ServicePosting>
    {
        public void Configure(EntityTypeBuilder<ServicePosting> builder)
        {
            builder.ToTable("SERVICE_POSTINGS");

            builder.HasKey(p => p.PostId);

            builder.Property(p => p.PostType).IsRequired().HasMaxLength(20);
            builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Description).IsRequired();

            builder.HasOne(p => p.User)
                .WithMany(u => u.Postings)
                .HasForeignKey(p => p.UserId);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Postings)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
