using BookAPI.Data;
using BookAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : Controller
    {
        private readonly BookAPIContext _context;

        public AuthorController(BookAPIContext context)
        {
            _context = context;
        }

        // GET: authorController
        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            _context.Author.ToList();
            return Ok(await _context.Author.ToListAsync());
        }

        //GET: Author/Create
        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetAuthor([FromRoute] string name)
        {
            var author = await _context.Author.FirstOrDefaultAsync(a=>a.Name==name);
            if (author == null) 
            {
                return NotFound("Not Found");
            }

            return Ok(author);
        }

        //create new author
        [HttpPost]
        public async Task<IActionResult> AddAuthor(AddAuthor author)
        {
            var authorNew = new AuthorModel()
            {
                Id = Guid.NewGuid(),
                Name = author.Name,
                Gender = author.Gender,
            };

            await _context.Author.AddAsync(authorNew);
            await _context.SaveChangesAsync();

            return Ok();
        }
        // Put Update
        [HttpPut]
        [Route("{name}")]
        public async Task<IActionResult> Edit([FromRoute] string name, AddAuthor updateAuth)
            {
            var authExists = await _context.Author.FirstOrDefaultAsync(a => a.Name == name);

            if (authExists != null)
                {
                authExists.Name = updateAuth.Name;
                authExists.Gender = updateAuth.Gender;

                 await _context.SaveChangesAsync();   
                return Ok(authExists);
                }

            return NotFound();
            }

        //Delete
        [HttpDelete]
        [Route("{name}")]
        public async Task<IActionResult> DeleteAuthor([FromRoute] string name)
        {
            var contact = await _context.Author.FirstOrDefaultAsync(a => a.Name == name);

            if (contact != null)
            {
                _context.Remove(contact);
                await _context.SaveChangesAsync();
                return Ok("Deleted successfully");
            }

            return NotFound();


        }
        /*
            // POST: Author/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Gender")] AuthorModel authorModel)
            {
                if (id != authorModel.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(authorModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AuthorModelExists(authorModel.Id))
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
                return View(authorModel);
            }

            // GET: Author/Delete/5
            public async Task<IActionResult> Delete(Guid? id)
            {
                if (id == null || _context.Author == null)
                {
                    return NotFound();
                }

                var authorModel = await _context.Author
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (authorModel == null)
                {
                    return NotFound();
                }

                return View(authorModel);
            }
            // POST: Author/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(Guid id)
            {

                var authorModel = await _context.Author.FindAsync(id);
                if (authorModel != null)
                {
                    _context.Author.Remove(authorModel);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            private bool AuthorModelExists(Guid id)
            {
                return (_context.Author?.Any(e => e.Id == id)).GetValueOrDefault();
            }*/
    }
}
