using Library.Application.Authors;
using Library.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Library.Application.DTOs;
using Microsoft.AspNetCore.Authorization;


namespace Library.API.Controllers
{
    public class AuthorController : BaseApiController
    {
        private readonly IMediator _mediator;
        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet] //api/authors
        public async Task<ActionResult<List<AuthorListDto>>> GetAuthors()
        {
            var result = await _mediator.Send(new AuthorList.Query());

            if (result == null || !result.IsSuccess)
            {
                return BadRequest();
            }

            return Ok(result.Value);
        }

        [HttpGet("{id}")] //api/authors/{id}
        public async Task<ActionResult<AuthorDto>> GetAuthor(Guid id)
        {
            var result = await _mediator.Send(new AuthorDetails.Query { Id = id });
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
        [HttpPut("{id}")] //api/authors/id z ciałem JSON obiektu Author
        public async Task<IActionResult> EditAuthor(Guid id, AuthorCreateDto author)
        {
            var command = new AuthorEdit.Command
            {
                Id = id,
                AuthorCreateDto = author
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
        [HttpPost] //api/authors
        public async Task<ActionResult> CreateAuthor(AuthorCreateDto author)
        {
            var result = await _mediator.Send(new AuthorCreate.Command { AuthorCreateDto = author });

            if (result == null) 
            { 
                return BadRequest();
            }

            if (result.IsSuccess && result.Value != null)
            {
                return CreatedAtAction(nameof(GetAuthor), new { id = result.Value.Id }, result.Value);
            }
            return BadRequest(result.ErrorMessage);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")] //api/authors/id
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            var result = await _mediator.Send(new AuthorDelete.Command { Id = id });
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
