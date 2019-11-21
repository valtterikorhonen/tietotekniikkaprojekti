using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdManagerWebApp.Models
{
    public class PasswordReset
    {
        public int Id { get; set; }
        public string code { get; set; }
        public string user { get; set; }
    }
}
