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
            public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
            {
                public void Configure(EntityTypeBuilder<WalletTransaction> builder)
                {
                    builder.ToTable("WALLET_TRANSACTIONS");

                    builder.HasKey(wt => wt.WalletTransactionId);

                    builder.Property(wt => wt.Type).IsRequired().HasMaxLength(20);
                    builder.Property(wt => wt.SourceType).IsRequired().HasMaxLength(50);

                    // 1. علاقة المحفظة -> Restrict
                    builder.HasOne(wt => wt.Wallet)
                        .WithMany(w => w.Transactions)
                        .HasForeignKey(wt => wt.WalletId)
                        .OnDelete(DeleteBehavior.Restrict);

                    // 2. علاقة معاملة الخدمة -> Restrict (لمنع الـ Multiple Paths)
                    builder.HasOne(wt => wt.ServiceTransaction)
                        .WithMany(st => st.WalletTransactions)
                        .HasForeignKey(wt => wt.ServiceTransactionId)
                        .OnDelete(DeleteBehavior.Restrict);

                    // 3. علاقة فاتورة الشراء -> Restrict (عشان الـ Error الحالي يختفي تماماً)
                    builder.HasOne(wt => wt.Invoice)
                        .WithMany(i => i.WalletTransactions)
                        .HasForeignKey(wt => wt.InvoiceId)
                        .OnDelete(DeleteBehavior.Restrict);
                }
            }
        }