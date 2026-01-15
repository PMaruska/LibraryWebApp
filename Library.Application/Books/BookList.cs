using Library.Domain;
using Library.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Books
{
    public class BookList
    {
        public class Query : IRequest<List<Book>> { }

        public class Handler : IRequestHandler<Query, List<Book>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<List<Book>> Handle(Query request, CancellationToken cancellationToken)
            {
                var books = await _context.Books
                    .Include(b => b.Author) // Dołącz autora do książki
                    .ToListAsync(cancellationToken);
                return books;
            }
        }   
    }
}
