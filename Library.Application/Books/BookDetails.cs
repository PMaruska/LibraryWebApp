using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain;
using Library.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Books
{
    public class BookDetails
    {
        public class Query : IRequest<Result<Book>>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<Book>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Result<Book>> Handle(Query request, CancellationToken cancellationToken)
            {
                var book = await _context.Books
                    .Include(b => b.Author) // Dołącz autora do książki
                    .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
                return Result<Book>.Success(book);
            }
        }
    }
}
