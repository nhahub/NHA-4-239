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
    public class PostApplicationConfiguration : IEntityTypeConfiguration<PostApplication>
    {
        public void Configure(EntityTypeBuilder<PostApplication> builder)
        {
            builder.ToTable("POST_APPLICATIONS");

            builder.HasKey(pa => pa.ApplicationId);

            builder.Property(pa => pa.Status).IsRequired().HasMaxLength(20).HasDefaultValue("Pending");

            builder.HasOne(pa => pa.Post)
                .WithMany(p => p.Applications)
                .HasForeignKey(pa => pa.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pa => pa.ApplicantUser)
                .WithMany(u => u.Applications)
                .HasForeignKey(pa => pa.ApplicantUserId)
                .OnDelete(DeleteBehavior.Restrict); // كسر الـ Cascade path هنا
        }
    }
}
