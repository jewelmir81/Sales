using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Models
{
    [Table("Order")]
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required, DataType(DataType.Date), Column(TypeName = "date"), Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Required(AllowEmptyStrings = true), Display(Name = "Order Number")]
        public string OrderNumber { get; set; }

        [ForeignKey("Customer"), Display(Name = "Customer")]
        public long CustomerId { get; set; }

        [Column(TypeName = "money"), DataType(DataType.Currency), Display(Name = "Total Amount"), DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal TotalAmount { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual IList<OrderItem> OrderItems { get; set; }
    }
}
