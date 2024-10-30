using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoviesLibraryData;

namespace Reviews.Pages
{
    public class testGetAllMoviesWithReviewsModelModel(MovieContext context) : PageModel
    {
        private MovieContext context = context;
        public void OnGet()
        {
            var movies = context.Movies.Include(m => m.Reviews).ToList();
            movies.ForEach(movie =>
            {
                Console.WriteLine($"Movie Id:{movie.Id}");
                Console.WriteLine($"Movie Title:{movie.Title}");
                Console.WriteLine($"Movie Year:{movie.Director}");
                Console.WriteLine($"Movie Poster:{movie.Description}");
                Console.WriteLine($"Movie {movie.Id} reviews:");
                Console.WriteLine();

                var reviews = movie.Reviews.ToList();
                reviews.ForEach(review =>
                {
                    Console.WriteLine($" Review Id:{review.Id}");
                });

            });


        }
    }
}
