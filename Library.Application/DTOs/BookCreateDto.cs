using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Library.Domain;

namespace Library.Application.DTOs
{
    public class BookCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public BookGenre Genre { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime PublishedDate { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public int PageCount { get; set; }
        public string Publisher { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }
}
