using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Library.Application.DTOs;

namespace Library.Application.Authors
{
    public class AuthorDetails
    {
        public class Query : IRequest<Result<AuthorDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<AuthorDto>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Result<AuthorDto>> Handle(Query request, CancellationToken cancellationToken)
            {

                var authorDto = await _context.Authors
                    .Where(a => a.Id == request.Id)
                    .Select(a => new AuthorDto
                    {
                        Id = a.Id,
                        FullName = $"{a.FirstName} {a.LastName}",
                        Biography = a.Biography,
                        Nationality = a.Nationality,
                        DateOfBirth = a.DateOfBirth,

                        Books = a.Books.Select(b => new BookSimpleDto
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Year = b.PublishedDate.Year,
                            Genre = b.Genre.ToString()
                        }).ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (authorDto == null)
                {
                    return Result<AuthorDto>.Failure("Author not found");
                }

                return Result<AuthorDto>.Success(authorDto);
            }
        }
    }
}
