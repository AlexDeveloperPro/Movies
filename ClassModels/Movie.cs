using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ClassModels
{
    public class Movie
    {
        public int Id { get; set; }
        [Required, MinLength(3, ErrorMessage = "Some message")]
        public string Title { get; set; } = "";
        [Required, MinLength(4)]
        public string Director { get; set; } = "";
        public string Description { get; set; } = "";
        public string PosterUrl { get; set; } = "";


        [ValidateNever]
        public ICollection<Genre> GenreList { get; set; }
        [ValidateNever]
        public ICollection<Review> Reviews { get; set; }

    }

    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ValidateNever]
        public ICollection<Movie> Movies { get; set; }
    }
}


