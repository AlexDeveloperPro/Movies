using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ClassModels;

namespace MoviesLibraryData
{
    public class MovieContext(DbContextOptions<MovieContext> options) : DbContext(options)
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Movie) //от ревью идем к муви (у одного фильма много ревью)
                .WithMany(m => m.Reviews) // путь обратно 
                .HasForeignKey(r => r.MovieId) //внешний ключ
                .IsRequired() //он должен быть обязательным (не нулем)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);// настройка связей между двумя базами
        }
    }
}





