
using LibraryApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class BooksController : ControllerBase
    {
        private LibraryDataContext _context;

        public BooksController(LibraryDataContext context)
        {
            _context = context;
        }

        [HttpGet("/books")]
        public async Task<ActionResult> GetAllBooks()
        {
            var response = await _context.Books.ToListAsync();
            return Ok(response);
        }
    }
}
