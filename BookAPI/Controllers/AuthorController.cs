﻿using BookAPI.Data;
using BookAPI.Models;
using Microsoft.AspNetCore.Http;
using BookAPI.ViewModel;
using BookAPI.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BookAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        [HttpGet("{name}")]
        public async Task<IActionResult> GetAuthor([FromQuery] string name)
        {
            var author = await _context.Author.FirstOrDefaultAsync(a=>a.Name==name);
            if (author == null) 
            {
                return NotFound(Message.NOT_FOUND);
            }

            return Ok(author);
        }

        //create new author
        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromBody]AuthorViewModel author)
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
        public async Task<IActionResult> Edit([FromBody]AuthorViewModel updateAuth)
            {
            var authExists = await _context.Author.FirstOrDefaultAsync(a=>a.Id == updateAuth.AuthorId);

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
        public async Task<IActionResult> DeleteAuthor([FromBody] Guid id)
        {
            var contact = await _context.Author.FirstOrDefaultAsync(a => a.Id == id);

            if (contact != null)
            {
                _context.Remove(contact);
                await _context.SaveChangesAsync();
                return Ok(Message.DELETION);
            }

            return NotFound();


        }
       
    }
}
