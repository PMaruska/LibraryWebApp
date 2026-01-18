using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Application.DTOs;
using FluentValidation;
using Library.Domain;
using Library.Infrastructure;

namespace Library.Application.Authors
{
    public class AuthorCreate
    {
        public class Command : IRequest<Result<AuthorDto>>
        {
            public required AuthorCreateDto AuthorCreateDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.AuthorCreateDto).SetValidator(new AuthorValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<AuthorDto>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Result<AuthorDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var author = new Author
                {
                    Id = Guid.NewGuid(),
                    FirstName = request.AuthorCreateDto.FirstName,
                    LastName = request.AuthorCreateDto.LastName,
                    Biography = request.AuthorCreateDto.Biography ?? string.Empty,
                    Nationality = request.AuthorCreateDto.Nationality ?? string.Empty,
                    DateOfBirth = request.AuthorCreateDto.DateOfBirth ?? null
                };

                _context.Authors.Add(author);
                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!success) return Result<AuthorDto>.Failure("Failed to create author");

                var authorDto = new AuthorDto
                {
                    Id = author.Id,
                    FullName = author.FullName,
                    Biography = author.Biography,
                    Nationality = author.Nationality,
                    DateOfBirth = author.DateOfBirth
                };

                return Result<AuthorDto>.Success(authorDto);

            }
        }
    }
}
