using BookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Data
{
    public class BookAPIContext : DbContext
    {


        public BookAPIContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<BookModel> Books { get; set; }
        public DbSet<AuthorModel> Author { get; set; }
    }

}


