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
    public class AuthorList
    {
        public class Query : IRequest<Result<List<AuthorListDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<AuthorListDto>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Result<List<AuthorListDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _context.Authors
                    .Select(a => new AuthorListDto
                    {
                        Id = a.Id,
                        FullName = a.FullName,
                        Nationality = a.Nationality,
                        DateOfBirth = a.DateOfBirth,
                        BooksCount = a.Books.Count() 
                    })
                    .ToListAsync(cancellationToken);

                return Result<List<AuthorListDto>>.Success(result);
            }
        }
    }
}
