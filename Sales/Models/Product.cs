using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Models
{
    [Table("Product")]
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [ForeignKey("Supplier"), Display(Name = "Supplier")]
        public long SupplierId { get; set; }

        [Column(TypeName = "money"), DataType(DataType.Currency), Display(Name = "Unit Price"), DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal UnitPrice { get; set; }

        public string Package { get; set; }

        [Display(Name = "Is Discontinued")]
        public bool IsDiscontinued { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
