using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Models
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [ForeignKey("Order"), Display(Name = "Order")]
        public long OrderId { get; set; }

        [ForeignKey("Product"), Display(Name = "Product")]
        public long ProductId { get; set; }

        [Column(TypeName = "money"), DataType(DataType.Currency), Display(Name = "Unit Price"), DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal UnitPrice { get; set; }

        public long Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}
