using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain;
using Library.Infrastructure;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Library.Application.Books
{
    public class BookCreate
    {
        public class Command : IRequest<Result<Book>>
        {
            public required Book Book { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Book).SetValidator(new BookValidator());
            }
        }

        public class  Handler : IRequestHandler<Command, Result<Book>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Book>> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Books.Add(request.Book);
                var success = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (!success)
                {
                    return Result<Book>.Failure("Failed to create book.");
                }
                return Result<Book>.Success(request.Book);
            }
        }

        
    }
}
