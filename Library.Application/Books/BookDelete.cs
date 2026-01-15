using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Infrastructure;
using MediatR;

namespace Library.Application.Books
{
    public class BookDelete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
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
                var book = await _context.Books.FindAsync(request.Id);
                if (book == null)
                {
                    return Result<Unit>.Failure("Book not found");
                }
                _context.Books.Remove(book);
                var success = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (!success)
                {
                    return Result<Unit>.Failure("Failed to delete the book");
                }
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
