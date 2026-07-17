using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouraDAL.Entities
{
    public class TimePurchaseInvoice
    {
        [Key]
        public Guid InvoiceId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required]
        public int PackageId { get; set; }
        [ForeignKey(nameof(PackageId))]
        public virtual FiatPackage Package { get; set; } = null!;

        [Required]
        public decimal AmountPaid { get; set; }
        [Required]
        public int HoursCredited { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        [Required, MaxLength(20)]
        public string PaymentStatus { get; set; } = "Pending"; // Success, Failed, Pending

        public virtual ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
    }
}
