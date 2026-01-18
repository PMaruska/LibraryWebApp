using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Library.Domain;
using Library.Infrastructure;
using Library.Application.DTOs;

namespace Library.Application.Authors
{
    public class AuthorValidator : AbstractValidator<AuthorCreateDto>
    {
        public AuthorValidator()
        {
            RuleFor(author => author.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
            RuleFor(author => author.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
            RuleFor(author => author.Biography).MaximumLength(2000).WithMessage("Biography cannot exceed 2000 characters.");
            RuleFor(author => author.Nationality).MaximumLength(50).WithMessage("Nationality cannot exceed 50 characters.");
            RuleFor(author => author.DateOfBirth)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Date of birth cannot be in the future.");
        }
    }
}
