﻿using AdManagerWebApp.Models;
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
                DisplayName = user.DisplayName
            };
        }
    }
}
