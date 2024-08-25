using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_HW11.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdAuthor { get; set; }
        public virtual Author Author { get; set; }
    }
}
