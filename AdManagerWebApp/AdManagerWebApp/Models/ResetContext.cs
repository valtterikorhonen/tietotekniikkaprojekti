using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdManagerWebApp.Models
{
    public class ResetContext : DbContext
    {

        public DbSet<PasswordReset> Resets { get; set; }
    }
}
