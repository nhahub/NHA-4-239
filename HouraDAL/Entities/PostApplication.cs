using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouraDAL.Entities
{
    public class PostApplication
    {
        [Key]
        public Guid ApplicationId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PostId { get; set; }
        [ForeignKey(nameof(PostId))]
        public virtual ServicePosting Post { get; set; } = null!;

        [Required]
        public Guid ApplicantUserId { get; set; }
        [ForeignKey(nameof(ApplicantUserId))]
        public virtual User ApplicantUser { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<ServiceTransaction> ServiceTransactions { get; set; } = new List<ServiceTransaction>();
    }
}