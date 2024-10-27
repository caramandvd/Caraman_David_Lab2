using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Caraman_David_Lab2.Models;

namespace Caraman_David_Lab2.Data
{
    public class Caraman_David_Lab2Context : DbContext
    {
        public Caraman_David_Lab2Context (DbContextOptions<Caraman_David_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Caraman_David_Lab2.Models.Book> Book { get; set; } = default!;
        public DbSet<Caraman_David_Lab2.Models.Publisher> Publisher { get; set; } = default!;
        public DbSet<Caraman_David_Lab2.Models.Author> Author { get; set; } = default!;
    }
}
