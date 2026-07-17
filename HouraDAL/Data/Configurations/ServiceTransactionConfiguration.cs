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
    public class ServiceTransactionConfiguration : IEntityTypeConfiguration<ServiceTransaction>
    {
        public void Configure(EntityTypeBuilder<ServiceTransaction> builder)
        {
            builder.ToTable("SERVICE_TRANSACTIONS");

            builder.HasKey(st => st.TransactionId);

            builder.Property(st => st.Status).IsRequired().HasMaxLength(20).HasDefaultValue("Pending");

            builder.HasOne(st => st.Post)
                .WithMany(p => p.ServiceTransactions)
                .HasForeignKey(st => st.PostId);

            builder.HasOne(st => st.Application)
                .WithMany(a => a.ServiceTransactions)
                .HasForeignKey(st => st.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            // حل مشكلة تعدد المسارات مع جدول الـ Users عن طريق الـ Restrict
            builder.HasOne(st => st.Provider)
                .WithMany(u => u.ProvidedTransactions)
                .HasForeignKey(st => st.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(st => st.Receiver)
                .WithMany(u => u.ReceivedTransactions)
                .HasForeignKey(st => st.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
