
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryApi.Domain;
using LibraryApi.Models.Books;
using Microsoft.AspNetCore.Http;
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
        private IMapper _mapper;
        private MapperConfiguration _mapperConfig;

        public BooksController(LibraryDataContext context, IMapper mapper, MapperConfiguration mapperConfig)
        {
            _context = context;
            _mapper = mapper;
            _mapperConfig = mapperConfig;
        }


        [HttpPost("books")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<GetBookDetailsResponse>> AddABook([FromBody] PostBookCreate bookToAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                // add it to the db context. (we need to make it a book)
                var book = _mapper.Map<Book>(bookToAdd); // PostBookCreate -> Book

                _context.Books.Add(book); // book.Id = 0;
                // save the changes to the database.
                await _context.SaveChangesAsync();
                // book.Id = 8;
                var response = _mapper.Map<GetBookDetailsResponse>(book); // Book -> GetBookDetailsResponse
                
                // return a 201, with location header, with a copy of what they'd get from that location

                return CreatedAtRoute("books#getbyid", new { bookId = response.Id }, response);
            }
        }



        /// <summary>
        /// Allows you to get a list of our vast inventory of fine books    
        /// </summary>
        /// <returns>A list of books for you to peruse.</returns>
        [HttpGet("/books")]
        [Produces("application/json")]
        public async Task<ActionResult<GetBooksResponse>> GetAllBooks()
        {
            var response = new GetBooksResponse();

            var books = await _context.BooksInInventory()
                .ProjectTo<GetBooksResponseItem>(_mapperConfig)
                .ToListAsync();

            response.Data = books;

            return Ok(response);

        }

        /// <summary>
        /// Gives you a book for a specific id.
        /// </summary>
        /// <param name="bookId">The id of the book</param>
        /// <returns>Either details about the book or a 404</returns>
        [HttpGet("/books/{bookId:int}", Name = "books#getbyid")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetBookDetailsResponse>> GetBookById([FromRoute] int bookId)
        {
            var book = await _context.BooksInInventory()
                .Where(b => b.Id == bookId)
                .ProjectTo<GetBookDetailsResponse>(_mapperConfig)
                .SingleOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(book);
            }
        }


    }
}
