using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouraDAL.Entities
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Required, MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public string? Bio { get; set; } // هيغذي الـ aboutMe
        public string? Skills { get; set; } // هيتخزن كـ "C#,MVC,Design" ونفصله في الـ Controller كـ Tags

        // Navigation Properties
        public virtual TimeWallet Wallet { get; set; } = null!;
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<ServicePosting> Postings { get; set; } = new List<ServicePosting>();
        public virtual ICollection<PostApplication> Applications { get; set; } = new List<PostApplication>();

        // العلاقات الثنائية مع المعاملات والتقييمات
        public virtual ICollection<ServiceTransaction> ProvidedTransactions { get; set; } = new List<ServiceTransaction>();
        public virtual ICollection<ServiceTransaction> ReceivedTransactions { get; set; } = new List<ServiceTransaction>();
        public virtual ICollection<Review> WrittenReviews { get; set; } = new List<Review>();
        public virtual ICollection<Review> ReceivedReviews { get; set; } = new List<Review>();
        public virtual ICollection<TimePurchaseInvoice> Invoices { get; set; } = new List<TimePurchaseInvoice>();
    }
}