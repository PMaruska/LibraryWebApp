using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs
{
    public class AuthorListDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string? Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int BooksCount { get; set; } // Tylko liczba!
    }
}
