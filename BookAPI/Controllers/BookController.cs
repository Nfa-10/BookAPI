using BookAPI.Data;
using BookAPI.Models;
using BookAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly BookAPIContext _context;

        public BookController(BookAPIContext context)
        {
            _context = context;
        }

        // GET: BookController
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            _context.Books.ToList();
            return Ok(await _context.Books.ToListAsync());
        }

        // GET: BookController/Details/5
        [HttpGet]
        [Route("{title}")]
        public async Task<IActionResult> GetBook([FromRoute] string title)
        {
            var book = await _context.Books.FirstOrDefaultAsync(a => a.Title == title);
            if (book == null)
            {
                return NotFound(Constants.NotFound);
            }
            return Ok(book);
        }
        
        // GET: BookController/Create
        //create new book
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookViewModel book)
        {
            var bookNew = new BookModel()
            {
                AuthorID = book.AuthorID,
                BookId = Guid.NewGuid(),
                Title = book.Title,
                dateOfPublishing = book.dateOfPublishing,
                Edition = book.Edition,
                Price = book.Price,
            };
            await _context.Books.AddAsync(bookNew);
            await _context.SaveChangesAsync();
            return Ok();
        }
       
        // Put Update
        [HttpPut]
        [Route("{title}")]
        public async Task<IActionResult> Edit([FromRoute] string title, [FromBody] BookViewModel updateBook)
        {
            var bookExists = await _context.Books.FirstOrDefaultAsync(a => a.Title == title);

            if (bookExists != null)
            {
                bookExists.Title = updateBook.Title;
                bookExists.dateOfPublishing = updateBook.dateOfPublishing;
                bookExists.Edition = updateBook.Edition;
                bookExists.Price = updateBook.Price;

                await _context.SaveChangesAsync();
                return Ok(bookExists);
            }
            return NotFound();
        }
       
       //Delete
       [HttpDelete]
       [Route("{title}")]
       public async Task<IActionResult> DeleteBook([FromRoute] string title)
       {
           var book = await _context.Books.FirstOrDefaultAsync(a => a.Title == title);

           if (book != null)
           {
               _context.Remove(book);
               await _context.SaveChangesAsync();
               return Ok(Constants.Deletion);
           }

           return NotFound();


       }
      
    }
}
