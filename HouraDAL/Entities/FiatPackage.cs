using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouraDAL.Entities
{
    public class FiatPackage
    {
        [Key]
        public int PackageId { get; set; }
        [Required, MaxLength(100)]
        public string PackageName { get; set; } = string.Empty;
        [Required]
        public int HoursCount { get; set; }
        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<TimePurchaseInvoice> Invoices { get; set; } = new List<TimePurchaseInvoice>();
    }
}