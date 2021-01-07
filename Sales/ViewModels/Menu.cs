using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.ViewModels
{
    public class Menu
    {
        [Display(Name = "Role")]
        public string RoleName { get; set; }

        [Display(Name = "Controller")]
        public string ControllerName { get; set; }

        [Display(Name = "Action")]
        public string ActionName { get; set; }

        public Menu() { }

        public Menu(string roleName, string controllerName, string actionName):this()
        {
            RoleName = roleName;
            ControllerName = controllerName;
            ActionName = actionName;
        }
    }
}
