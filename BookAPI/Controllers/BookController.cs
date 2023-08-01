using BookAPI.Data;
using BookAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Controllers
{
    public class BookController : Controller
    {
        private readonly BookAPIContext _context;

        public BookController(BookAPIContext context)
        {
            _context = context;
        }

        // GET: BookController
        public async Task<IActionResult> Index(Guid? id, string searchString)
        {
            List<AuthorModel> authorList = _context.Author.ToList();

            ViewBag.Authors = new SelectList(authorList, "Id", "Name");



            if (id != null || !String.IsNullOrEmpty(searchString))
            {
                var books = _context.Books.Include(b => b.Author).Where(b => b.Author.Id == id || b.Title!.Contains(searchString));


                if (!books.Any())
                {
                    ViewBag.Message = "Book Not Found";
                    return View(await books.ToListAsync());
                }
                return View(await books.ToListAsync());
            }
            ViewBag.SelectedAuthorId = id;
            return View(await _context.Books.ToListAsync());
        }

        // GET: BookController/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var bookModel = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        // GET: BookController/Create
        public IActionResult Create()
        {
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Name");
            return View();

        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Title,Edition,Price,dateOfPublishing,AuthorID")] BookModel bookModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bookModel.BookId = Guid.NewGuid();
                    _context.Add(bookModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(bookModel);
            }
            catch
            {
                ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Name", bookModel.AuthorID);
                return View(bookModel);
            }
        }

        // GET: BookController/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var bookModel = await _context.Books.FindAsync(id);
            if (bookModel == null)
            {
                return NotFound();
            }
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Name", bookModel.AuthorID);
            return View(bookModel);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BookId,Title,Edition,Price,dateOfPublishing,AuthorID")] BookModel bookModel)
        {
            if (id != bookModel.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookModelExists(bookModel.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Name", bookModel.AuthorID);
            return View(bookModel);
        }

        // GET: BookController/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var bookModel = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }
        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookDbContext.Books'  is null.");
            }
            var bookModel = await _context.Books.FindAsync(id);
            if (bookModel != null)
            {
                _context.Books.Remove(bookModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookModelExists(Guid id)
        {
            return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
    }
}
