using FluentValidation;
using Library.Domain;
using Library.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Books
{
    public class BookEdit
    {
        public class Command : IRequest<Result<Unit>>
        { 
            public required Book Book { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Book).SetValidator(new BookValidator());
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
                var book = await _context.Books.FindAsync(request.Book.Id);
                if (book == null) return null;

                // Aktualizuj właściwości książki
                book.Title = request.Book.Title;
                book.Description = request.Book.Description;
                book.Genre = request.Book.Genre;
                book.AuthorId = request.Book.AuthorId;
                book.PublishedDate = request.Book.PublishedDate;
                book.ISBN = request.Book.ISBN;
                book.PageCount = request.Book.PageCount;
                book.Publisher = request.Book.Publisher;
                book.IsAvailable = request.Book.IsAvailable;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to update book");

                return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}
