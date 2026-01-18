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
            public required BookCreateDto BookCreateDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.BookCreateDto).SetValidator(new BookValidator());
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

                if (await _context.Authors.FindAsync(new object?[] { request.BookCreateDto.AuthorId }, cancellationToken) is null)
                {
                    return Result<Unit>.Failure("Author not found.");
                }

                book.Title = request.BookCreateDto.Title;
                book.Description = request.BookCreateDto.Description ?? string.Empty; 
                book.Genre = request.BookCreateDto.Genre;
                book.AuthorId = request.BookCreateDto.AuthorId; 
                book.PublishedDate = request.BookCreateDto.PublishedDate;
                book.ISBN = request.BookCreateDto.ISBN;
                book.PageCount = request.BookCreateDto.PageCount;
                book.Publisher = request.BookCreateDto.Publisher;
                book.IsAvailable = request.BookCreateDto.IsAvailable;

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