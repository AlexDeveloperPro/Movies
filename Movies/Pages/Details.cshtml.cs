using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClassModels;
using Microsoft.AspNetCore.Identity;
using MoviesLibraryData;

namespace Reviews.Areas.Identity;
//namespace Movies.Pages;

public class DetailsModel : PageModel
{
    private MovieContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public DetailsModel(MovieContext context, UserManager<IdentityUser> userManager)

    {
        _context = context;
        _userManager = userManager;
    }

    public Movie Movie { get; set; } = default!;
    public List<Review> ReviewsList { get; set; } = new List<Review>();
    public List<Review> CurrentUserReview { get; set; } = new List<Review>();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var movie = await _context.Movies.Include(m => m.Reviews).FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
        {
            return NotFound();
        }
        else
        {
            Movie = movie;
            CurrentUserReview = movie.Reviews.Where(r => r.Author == _userManager.GetUserName(User)).ToList(); // отзыв залогиненого автора
            ReviewsList = movie.Reviews.Where(r => r.Author != _userManager.GetUserName(User)).ToList(); // все остальные отзывы
            //var user = await _userManager.GetUserAsync(User);
            //if (await _userManager.IsInRoleAsync(user, "admin"))
            //{

            //}
        }
        return Page();
    }
    [BindProperty]
    public Review Review { get; set; } = default!;

    // For more information, see https://aka.ms/RazorPagesCRUD. 


    public async Task<IActionResult> OnPostAsync(int id)
    {
        Console.WriteLine(RouteData.Values["id"]);
        var current_movie = _context.Movies.Include(m => m.Reviews).FirstOrDefault(m => m.Id == id);
        if (current_movie is null)
        {
            return NotFound();
        }

        //установить дату и автора и к какому фильму он относится 
        Review.DatePublished = DateTime.Now;
        Review.MovieId = id;
        Review.Author = _userManager.GetUserName(User);
        Review.Movie = current_movie;
       
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Error");
        }

        _context.Reviews.Add(Review);
        await _context.SaveChangesAsync();

        return RedirectToPage("Details", new { id = id });
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var review = await _context.Reviews.FindAsync(id);
        if (user != null && review != null && (user.Email == review.Author || User.IsInRole("admin")))
        {
            Review = review;
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage("Details", new { id = Review.MovieId });     
    }

    public async Task<IActionResult> OnPostEditAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Movie).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(Movie.Id))
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
    private bool MovieExists(int id)
    {
        return _context.Movies.Any(e => e.Id == id);
    }
}


/*[Authorize(Roles = "admin")]*/