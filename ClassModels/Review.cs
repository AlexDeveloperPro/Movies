using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClassModels
{
    public class Review
    {
        public int Id { get; set; }

        [Required, Length(10, 50, ErrorMessage = "Ошибка: поле {0} должно быть от {1} до {2}.")]
        public string Author { get; set; } = "";

        [Required, Length(1, 50, ErrorMessage = "Ошибка: поле {0} должно быть от {1} до {2}.")]
        public string Content { get; set; } = "";

        [Required, Range(1, 5, ErrorMessage = "Ошибка: поле {0} должно быть от {1} до {2}.")]
        public int Rating { get; set; }

        public DateTime DatePublished { get; set; }

        [ValidateNever]
        public int MovieId { get; set; }
        [ValidateNever]
        public Movie Movie { get; set; } = new Movie();
    }
}
