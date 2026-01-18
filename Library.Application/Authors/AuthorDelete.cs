using Library.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Authors
{
    public class AuthorDelete
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
                var author = await _context.Authors.FindAsync(new object[] { request.Id }, cancellationToken);
                if (author == null)
                {
                    return Result<Unit>.Failure("Author not found");
                }

                var hasBooks = await _context.Books.AnyAsync(b => b.AuthorId == request.Id, cancellationToken);

                if (hasBooks)
                {
                    return Result<Unit>.Failure("Cannot delete author who has books in the library. Remove books first.");
                }

                _context.Authors.Remove(author);
                var success = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (!success)
                {
                    return Result<Unit>.Failure("Failed to delete the author");
                }
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
