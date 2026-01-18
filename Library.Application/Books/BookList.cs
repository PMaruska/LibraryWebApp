using Library.Domain;
using Library.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Library.Application.DTOs;


namespace Library.Application.Books
{
    public class BookList
    {
        public class Query : IRequest<List<BookDto>> { }

        public class Handler : IRequestHandler<Query, List<BookDto>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<List<BookDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Ręczne mapowanie (w przyszłości można użyć AutoMapper)
                return await _context.Books
                    .Select(b => new BookDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Description = b.Description,
                        Genre = b.Genre.ToString(), // Enum -> String
                        PublishedDate = b.PublishedDate,
                        // Łączymy imię i nazwisko autora w jeden ciąg
                        AuthorName = $"{b.Author.FirstName} {b.Author.LastName}",
                        ISBN = b.ISBN,
                        PageCount = b.PageCount,
                        Publisher = b.Publisher,
                        IsAvailable = b.IsAvailable
                    })
                    .ToListAsync(cancellationToken);
            }
        }   
    }
}
