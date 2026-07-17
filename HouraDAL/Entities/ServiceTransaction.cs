using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouraDAL.Entities
{
    public class ServiceTransaction
    {
        [Key]
        public Guid TransactionId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ProviderId { get; set; }
        public virtual User Provider { get; set; } = null!;

        [Required]
        public Guid ReceiverId { get; set; }
        public virtual User Receiver { get; set; } = null!;

        [Required]
        public Guid PostId { get; set; }
        [ForeignKey(nameof(PostId))]
        public virtual ServicePosting Post { get; set; } = null!;

        // تعديل التميز: ربط المعاملة بالأبلكيشن المقبول مباشرة لتوثيق السيرفر
        public Guid? ApplicationId { get; set; }
        [ForeignKey(nameof(ApplicationId))]
        public virtual PostApplication? Application { get; set; }

        [Required]
        public int DurationInMinutes { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Completed, Cancelled, Disputed

        // إداريات الـ Admin في حالة النزاع
        public Guid? ResolvedByAdminId { get; set; }
        public string? ResolutionNotes { get; set; }

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
    }
}