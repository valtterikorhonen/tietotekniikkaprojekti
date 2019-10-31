using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdManagerWebApp.Models
{
    public class PasswordModel
    {
        public string Current { get; set; }
        public string New { get; set; }
        public string Repeat { get; set; }
    }
}
