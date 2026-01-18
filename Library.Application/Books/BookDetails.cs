using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain;
using Library.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Library.Application.DTOs;

namespace Library.Application.Books
{
    public class BookDetails
    {
        public class Query : IRequest<Result<BookDto>>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<BookDto>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Result<BookDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                // 2. Wyszukujemy i mapujemy w jednym zapytaniu
                var bookDto = await _context.Books
                    .Where(b => b.Id == request.Id) // Najpierw filtrujemy po ID (optymalizacja SQL)
                    .Select(b => new BookDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Description = b.Description,
                        Genre = b.Genre.ToString(), // Konwersja Enum -> String
                        PublishedDate = b.PublishedDate,
                        AuthorName = $"{b.Author.FirstName} {b.Author.LastName}", // Spłaszczamy Autora
                        ISBN = b.ISBN,
                        PageCount = b.PageCount,
                        Publisher = b.Publisher,
                        IsAvailable = b.IsAvailable
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                // 3. Sprawdzamy, czy książka została znaleziona
                if (bookDto == null)
                {
                    return Result<BookDto>.Failure("Book not found");
                }

                return Result<BookDto>.Success(bookDto);
            }
        }
    }
}
