using Library.Application.Books;
using Library.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Library.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Library.API.Controllers
{
    public class BookController : BaseApiController
    {
        private readonly IMediator _mediator;
        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }
  
        [HttpGet] //api/books
        public async Task<ActionResult<List<BookDto>>> GetBooks()
        {
            var result = await _mediator.Send(new BookList.Query());

            if (result == null || !result.IsSuccess)
                return BadRequest(); 

            return Ok(result.Value);
        }

        [HttpGet("{id}")] //api/books/{id}
        public async Task<ActionResult<BookDto>> GetBook(Guid id)
        {
            var result = await _mediator.Send(new BookDetails.Query { Id = id });

            if (result == null || result.Value == null)
            {
                return NotFound();
            }

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.ErrorMessage);
     
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")] //api/books/id z ciałem JSON obiektu Book
        public async Task<IActionResult> EditBook(Guid id, BookCreateDto book)
        {
            var command = new BookEdit.Command
            {
                Id = id,
                BookCreateDto = book
            };

            var result = await _mediator.Send(command);

            if (result == null) return NotFound();

            if (result.IsSuccess)
            {
                return Ok();
            }

            return BadRequest(result.ErrorMessage);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost] //api/books
        public async Task<ActionResult> CreateBook(BookCreateDto book)
        {
            var result = await _mediator.Send(new BookCreate.Command { BookCreateDto = book });
            if (result == null)
            {
                return BadRequest();
            }
            if (result.IsSuccess && result.Value != null)
            {
                return CreatedAtAction(nameof(GetBook), new { id = result.Value.Id }, result.Value);
            }
            return BadRequest(result.ErrorMessage);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")] //api/books/{id}
        public async Task<ActionResult> DeleteBook(Guid id)
        {
            var result = await _mediator.Send(new BookDelete.Command { Id = id });
            if (result == null)
            {
                return NotFound();
            }
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
