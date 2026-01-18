using FluentValidation;
using Library.Application.DTOs;
using Library.Domain;
using Library.Infrastructure;
using MediatR;

namespace Library.Application.Books
{
    public class BookEdit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
            public required BookCreateDto BookDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(DataContext context)
            {
                RuleFor(x => x.BookDto).SetValidator(new BookValidator(context));
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var book = await _context.Books.FindAsync(new object[] { request.Id }, cancellationToken);

                if (book == null)
                {
                    return Result<Unit>.Failure("Book not found");
                }

                if (await _context.Authors.FindAsync(new object?[] { request.BookDto.AuthorId }, cancellationToken) is null)
                {
                    return Result<Unit>.Failure("Author not found.");
                }

                book.Title = request.BookDto.Title;
                book.Description = request.BookDto.Description ?? string.Empty; 
                book.Genre = request.BookDto.Genre;
                book.AuthorId = request.BookDto.AuthorId; 
                book.PublishedDate = request.BookDto.PublishedDate;
                book.ISBN = request.BookDto.ISBN;
                book.PageCount = request.BookDto.PageCount;
                book.Publisher = request.BookDto.Publisher;
                book.IsAvailable = request.BookDto.IsAvailable;

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!result)
                {
                    return Result<Unit>.Failure("Failed to update book (or no changes detected)");
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}