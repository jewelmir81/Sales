using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Models
{
    [Table("City")]
    public class City
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Display(Name = "City")]
        public string Name { get; set; }

        [ForeignKey("Country"), Display(Name = "Country")]
        public long CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
