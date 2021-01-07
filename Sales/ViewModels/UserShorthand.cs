using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.ViewModels
{
    public class UserShorthand
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public UserShorthand()
        {

        }

        public UserShorthand(string id, string name) : this()
        {
            Id = id;
            Name = name;
        }
    }
}
