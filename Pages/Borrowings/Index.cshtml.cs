﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Caraman_David_Lab2.Data;
using Caraman_David_Lab2.Models;

namespace Caraman_David_Lab2.Pages.Borrowings
{
    public class IndexModel : PageModel
    {
        private readonly Caraman_David_Lab2.Data.Caraman_David_Lab2Context _context;

        public IndexModel(Caraman_David_Lab2.Data.Caraman_David_Lab2Context context)
        {
            _context = context;
        }

        public IList<Borrowing> Borrowing { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Borrowing = await _context.Borrowing
                .Include(b => b.Book)
                    .ThenInclude(b => b.Author)
                .Include(b => b.Member).ToListAsync();
        }
    }
}
