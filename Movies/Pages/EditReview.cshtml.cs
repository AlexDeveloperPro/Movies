using ClassModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClassModels;
using MoviesLibraryData;

//namespace Movies.Areas.Identity
namespace Reviews.Pages
{
    public class EditReviewModel : PageModel
    {
        private readonly MovieContext _context;

        public EditReviewModel(MovieContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Review Review { get; set; } = default!;

        [BindProperty]
        public string Content { get; set; }

        [BindProperty]
        public int Rating { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.Include(r => r.Movie).FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }
            Review = review;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            var review = await _context.Reviews.Include(r => r.Movie).FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }
            Review = review;

            Review.Content = Content;
            Review.Rating = Rating;

            _context.Attach(Review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(Review.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./Index");
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}





  
    