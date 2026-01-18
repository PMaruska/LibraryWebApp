using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain;
using Library.Application.DTOs;
using Library.Infrastructure;

namespace Library.Application.Books
{
    public class BookValidator : AbstractValidator<BookCreateDto>
    {
        public BookValidator()
        {
            RuleFor(book => book.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");
            RuleFor(book => book.AuthorId)
                .NotNull().WithMessage("AuthorId is required.");
            RuleFor(book => book.PublishedDate).NotNull().WithMessage("Published date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Published date cannot be in the future.");
            RuleFor(book => book.ISBN).NotNull().WithMessage("ISBN is required.")
                .Matches(@"^(97(8|9))?\d{9}(\d|X)$").WithMessage("ISBN must be a valid format.");
            RuleFor(book => book.PageCount).NotNull().WithMessage("Page count is required.")
                .GreaterThan(0).WithMessage("Page count must be greater than zero.");
            RuleFor(book => book.Publisher)
                .NotEmpty().WithMessage("Publisher is required.")
                .MaximumLength(100).WithMessage("Publisher cannot exceed 100 characters.");
            RuleFor(book => book.Genre)
                .IsInEnum().WithMessage("Genre must be a valid value.");
            RuleFor(book => book.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
        }
    }
}
