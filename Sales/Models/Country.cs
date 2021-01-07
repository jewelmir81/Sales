using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Models
{
    [Table("Country")]
    public class Country
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Display(Name = "Country")]
        public string Name { get; set; }

        public virtual IList<City> Cities { get; set; }
    }
}
