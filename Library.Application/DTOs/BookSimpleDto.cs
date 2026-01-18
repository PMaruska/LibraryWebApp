using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//DTO zawierający podstawowe informacje o książce na potrzeby listy książek autora

namespace Library.Application.DTOs
{
    public class BookSimpleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; } 
        public string Genre { get; set; } = string.Empty;
    }
}