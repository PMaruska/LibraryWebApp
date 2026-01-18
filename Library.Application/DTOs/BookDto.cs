using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }

        // Kluczowa zmiana: spłaszczamy obiekt autora do samego imienia i nazwiska
        public string AuthorName { get; set; } = string.Empty;

        public string ISBN { get; set; } = string.Empty;
        public int PageCount { get; set; }
        public string Publisher { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }
}
