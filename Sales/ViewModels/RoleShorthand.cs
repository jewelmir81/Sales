using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.ViewModels
{
    public class RoleShorthand
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public RoleShorthand()
        {

        }

        public RoleShorthand(string id, string name) : this()
        {
            Id = id;
            Name = name;
        }
    }
}
