using Library.Application.Books;
using Library.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<List<Book>>> GetBooks()
        {
            return await _mediator.Send(new BookList.Query());
        }

        [HttpGet("{id}")] //api/books/{id}
        public async Task<ActionResult<Book>> GetBook(Guid id)
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

        [HttpPut("{id}")] //api/books/id z ciałem JSON obiektu Book
        public async Task<IActionResult> EditBook(Guid id, Book book)
        {
            book.Id = id;
            var result = await _mediator.Send(new  BookEdit.Command { Book = book});
            if (result == null)
            {
                return NotFound();
            }
            if (result.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost] //api/books
        public async Task<ActionResult> CreateBook(Book book)
        {
            var result = await _mediator.Send(new BookCreate.Command { Book = book });
            if (result == null || !result.IsSuccess)
            {
                return BadRequest();
            }
            if (result.IsSuccess && result.Value != null)
            {
                return CreatedAtAction(nameof(GetBook), new { id = result.Value.Id }, result.Value);
            }
            return BadRequest(result.ErrorMessage);
        }

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
                return Ok();
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
