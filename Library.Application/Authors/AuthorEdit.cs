using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Library.Application.DTOs;
using Library.Domain;
using Library.Infrastructure;
using MediatR;


namespace Library.Application.Authors
{
    public class AuthorEdit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
            public required AuthorCreateDto AuthorCreateDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.AuthorCreateDto).SetValidator(new AuthorValidator());
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
                var author = await _context.Authors.FindAsync(new object[] { request.Id }, cancellationToken);
                if (author == null)
                {
                    return Result<Unit>.Failure("Author not found");
                }
                author.FirstName = request.AuthorCreateDto.FirstName;
                author.LastName = request.AuthorCreateDto.LastName;
                author.Biography = request.AuthorCreateDto.Biography ?? string.Empty;
                author.Nationality = request.AuthorCreateDto.Nationality ?? string.Empty;
                author.DateOfBirth = request.AuthorCreateDto.DateOfBirth ?? null;

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!result)
                {
                    return Result<Unit>.Failure("Failed to update author");
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
