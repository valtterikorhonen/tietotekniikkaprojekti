using AdManagerWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdManagerWebApp.Helpers
{
    public static class ModelHelpers
    {
        public static UserViewModel ToViewModel(this Alue71UserPrincipal user)
        {
            if (null == user)
            {
                return null;
            }

            return new UserViewModel
            {
                Name = user.Name,
                Email = user.EmailAddress,
                DisplayName = user.DisplayName,
                GivenName = user.GivenName,
                Surname = user.Surname,
                SamAccountName = user.SamAccountName,
                EmployeeType = user.Title
            };
        }

        public static Alue71UserPrincipal UpdateFromModel(this Alue71UserPrincipal principal, UserViewModel user)
        {
            if(null == user)
            {
                return principal;
            }
            if(!string.IsNullOrEmpty(user.Name))
                principal.Name = user.Name;
            if (!string.IsNullOrEmpty(user.Email))
                principal.EmailAddress = user.Email;
            if (!string.IsNullOrEmpty(user.DisplayName))
                principal.DisplayName = user.DisplayName;
            if (!string.IsNullOrEmpty(user.GivenName))
                principal.GivenName = user.GivenName;
            if (!string.IsNullOrEmpty(user.Surname))
                principal.Surname = user.Surname;
            if (!string.IsNullOrEmpty(user.SamAccountName))
                principal.SamAccountName = user.SamAccountName;
            if (!string.IsNullOrEmpty(user.EmployeeType))
                principal.Title = user.EmployeeType;

            return principal;
        }
    }
}
