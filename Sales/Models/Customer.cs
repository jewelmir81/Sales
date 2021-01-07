using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Display(Name = "Firs tName")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [ForeignKey("City"), Display(Name = "City")]
        public long CityId { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual City City { get; set; }
    }
}
