using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Models
{
    [Table("Supplier")]
    public class Supplier
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }

        [Display(Name = "Contact Title")]
        public string ContactTitle { get; set; }

        [ForeignKey("City"), Display(Name = "City")]
        public long CityId { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Fax { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
