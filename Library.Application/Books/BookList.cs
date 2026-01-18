using Library.Domain;
using Library.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Library.Application.DTOs;


namespace Library.Application.Books
{
    public class BookList
    {
        public class Query : IRequest<Result<List<BookDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<BookDto>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Result<List<BookDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _context.Books
                    .Select(b => new BookDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Description = b.Description,
                        Genre = b.Genre.ToString(), 
                        PublishedDate = b.PublishedDate,
                        AuthorName = $"{b.Author.FirstName} {b.Author.LastName}",
                        ISBN = b.ISBN,
                        PageCount = b.PageCount,
                        Publisher = b.Publisher,
                        IsAvailable = b.IsAvailable
                    })
                    .ToListAsync(cancellationToken);

                return Result<List<BookDto>>.Success(result);
            }
        }   
    }
}
