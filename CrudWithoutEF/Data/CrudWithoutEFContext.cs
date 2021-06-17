using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CrudWithoutEF.Models;

namespace CrudWithoutEF.Data
{
    public class CrudWithoutEFContext : DbContext
    {
        public CrudWithoutEFContext (DbContextOptions<CrudWithoutEFContext> options)
            : base(options)
        {
        }

        public DbSet<CrudWithoutEF.Models.BookViewModel> BookViewModel { get; set; }
    }
}
