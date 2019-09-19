using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace AdManagerWebApp.Models
{
    [DirectoryRdnPrefix("CN")]
    [DirectoryObjectClass("User")]
    public class Alue71UserPrincipal : UserPrincipal
    {
        public Alue71UserPrincipal(PrincipalContext context) : base(context)
        { }
    }
}
