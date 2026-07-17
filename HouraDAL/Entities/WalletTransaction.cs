using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouraDAL.Entities
{
    public class WalletTransaction
    {
        [Key]
        public Guid WalletTransactionId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid WalletId { get; set; }
        [ForeignKey(nameof(WalletId))]
        public virtual TimeWallet Wallet { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Type { get; set; } = string.Empty; // "Deposit" or "Withdraw"

        [Required]
        public int AmountInMinutes { get; set; }

        [Required]
        public int BalanceAfter { get; set; } // الأمان الكامل للحسابات

        [Required, MaxLength(50)]
        public string SourceType { get; set; } = string.Empty; // "Service" or "Purchase"

        // العلاقات الـ Nullable الصريحة لسهولة الـ LINQ Joins بدلاً من الـ Polymorphic Guid العام
        public Guid? ServiceTransactionId { get; set; }
        [ForeignKey(nameof(ServiceTransactionId))]
        public virtual ServiceTransaction? ServiceTransaction { get; set; }

        public Guid? InvoiceId { get; set; }
        [ForeignKey(nameof(InvoiceId))]
        public virtual TimePurchaseInvoice? Invoice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}