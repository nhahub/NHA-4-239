using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouraDAL.Entities
{
    public class Review
    {
        [Key]
        public Guid ReviewId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ServiceTransactionId { get; set; }
        [ForeignKey(nameof(ServiceTransactionId))]
        public virtual ServiceTransaction ServiceTransaction { get; set; } = null!;

        [Required]
        public Guid ReviewerUserId { get; set; }
        public virtual User Reviewer { get; set; } = null!;

        [Required]
        public Guid RevieweeUserId { get; set; }
        public virtual User Reviewee { get; set; } = null!;

        [Required]
        public int Rating { get; set; } // من 1 لـ 5 مثلاً

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}