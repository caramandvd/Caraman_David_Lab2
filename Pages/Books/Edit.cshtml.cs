using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Caraman_David_Lab2.Data;
using Caraman_David_Lab2.Models;
using Microsoft.AspNetCore.Authorization;

namespace Caraman_David_Lab2.Pages.Books
{
    [Authorize(Roles = "Admin")]
    public class EditModel : BookCategoriesPageModel
    {
        private readonly Caraman_David_Lab2.Data.Caraman_David_Lab2Context _context;

        public EditModel(Caraman_David_Lab2.Data.Caraman_David_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Book = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.Author)
                .Include(b => b.BookCategories).ThenInclude(b => b.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Book == null)
            {
                return NotFound();
            }

            // Populate categories and dropdowns for authors and publishers
            PopulateAssignedCategoryData(_context, Book);

            var authorList = await _context.Author
                .Select(x => new { x.Id, FullName = x.LastName + " " + x.FirstName })
                .ToListAsync();
            ViewData["AuthorID"] = new SelectList(authorList, "Id", "FullName", Book.AuthorID);
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName", Book.PublisherID);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookToUpdate = await _context.Book
                .Include(i => i.Publisher)
                .Include(i => i.Author)
                .Include(i => i.BookCategories).ThenInclude(i => i.Category)
                .FirstOrDefaultAsync(s => s.ID == id);

            if (bookToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Book>(
                bookToUpdate,
                "Book",
                i => i.Title, i => i.AuthorID, i => i.Price, i => i.PublishingDate, i => i.PublisherID))
            {
                UpdateBookCategories(_context, selectedCategories, bookToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            // Repopulate AssignedCategoryData if there is an error
            PopulateAssignedCategoryData(_context, bookToUpdate);
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "FullName", bookToUpdate.AuthorID);
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName", bookToUpdate.PublisherID);

            return Page();
        }
    }
}
