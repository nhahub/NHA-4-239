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
    public class TimeWalletConfiguration : IEntityTypeConfiguration<TimeWallet>
    {
        public void Configure(EntityTypeBuilder<TimeWallet> builder)
        {
            builder.ToTable("TIME_WALLETS");

            builder.HasKey(w => w.WalletId);

            builder.Property(w => w.BalanceInMinutes)
                .HasDefaultValue(0);

            // علاقة One-to-One مع المستخدم
            builder.HasOne(w => w.User)
                .WithOne(u => u.Wallet)
                .HasForeignKey<TimeWallet>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade); // لو اليوزر اتمسح محفظته تتمسح
        }
    }
}