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
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("REVIEWS");

            builder.HasKey(r => r.ReviewId);

            builder.Property(r => r.Comment).HasMaxLength(1000);

            builder.HasOne(r => r.ServiceTransaction)
                .WithMany(st => st.Reviews)
                .HasForeignKey(r => r.ServiceTransactionId);

            // كسر الـ Cascade paths لأن الـ Reviewer والـ Reviewee يشيرون لنفس الجدول (USERS)
            builder.HasOne(r => r.Reviewer)
                .WithMany(u => u.WrittenReviews)
                .HasForeignKey(r => r.ReviewerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Reviewee)
                .WithMany(u => u.ReceivedReviews)
                .HasForeignKey(r => r.RevieweeUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}