using Microsoft.AspNetCore.Identity;
using Sales.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.ViewModels
{
    public class UserRole
    {
        public UserShorthand User { get; set; }

        [Display(Name = "Assigned Roles")]
        public IList<RoleShorthand> AssignedRoles { get; set; }

        [Display(Name = "Unassigned Roles")]
        public IList<RoleShorthand> UnassignedRoles { get; set; }

        public string UserId { get; set; }

        public IList<string> RoleIds { get; set; }

        public UserRole()
        {

        }

        public UserRole(UserShorthand user, IList<RoleShorthand> assignedRoles, IList<RoleShorthand> unassignedRoles)
        {
            User = user;
            AssignedRoles = assignedRoles;
            UnassignedRoles = unassignedRoles;
        }
    }
}
