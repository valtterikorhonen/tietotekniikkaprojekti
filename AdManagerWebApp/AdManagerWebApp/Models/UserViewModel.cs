using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdManagerWebApp.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string EmployeeType { get; set; }
        public string SamAccountName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
    }
}
