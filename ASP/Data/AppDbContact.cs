using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ASP.Data
{
    public class AppDbContact : DbContext
    {
        public AppDbContact(DbContextOptions<AppDbContact> options) : base(options)
        {
        }
    }

  
}