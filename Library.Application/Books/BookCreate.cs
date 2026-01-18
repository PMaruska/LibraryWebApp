using MediatR;
using Library.Domain;
using Library.Infrastructure;
using FluentValidation;
using Library.Application.DTOs; 

namespace Library.Application.Books
{
    public class BookCreate
    {
        public class Command : IRequest<Result<BookDto>>
        {
            public required BookCreateDto BookCreateDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.BookCreateDto).SetValidator(new BookValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<BookDto>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<BookDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Authors.FindAsync(new object?[] { request.BookCreateDto.AuthorId }, cancellationToken) is null)
                {
                    return Result<BookDto>.Failure("Author not found.");
                }

                var book = new Book
                {
                    Id = Guid.NewGuid(),
                    Title = request.BookCreateDto.Title,
                    Description = request.BookCreateDto.Description,
                    Genre = request.BookCreateDto.Genre,
                    AuthorId = request.BookCreateDto.AuthorId,
                    PublishedDate = request.BookCreateDto.PublishedDate,
                    ISBN = request.BookCreateDto.ISBN,
                    PageCount = request.BookCreateDto.PageCount,
                    Publisher = request.BookCreateDto.Publisher,
                    IsAvailable = request.BookCreateDto.IsAvailable
                };

                _context.Books.Add(book);
                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!success) return Result<BookDto>.Failure("Failed to create book.");

                var resultDto = new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    Genre = book.Genre.ToString(),
                    PublishedDate = book.PublishedDate,
                    ISBN = book.ISBN,
                    PageCount = book.PageCount,
                    Publisher = book.Publisher,
                    IsAvailable = book.IsAvailable,
                    AuthorName = ""
                };

                return Result<BookDto>.Success(resultDto);
            }
        }
    }
}