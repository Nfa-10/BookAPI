using BookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Data
{
    public class BookAPIContext : DbContext
    {


        public BookAPIContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             modelBuilder.Entity<BookModel>()
                .HasOne(s => s.Author)
                .WithMany(s => s.Books)
                .HasForeignKey(s => s.AuthorID)
                .OnDelete(DeleteBehavior.NoAction);
        }
        public DbSet<BookModel> Books { get; set; }
        public DbSet<AuthorModel> Author { get; set; }
    }

}


