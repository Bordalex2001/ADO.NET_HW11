using ADO.NET_HW11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_HW11.ViewModels
{
    public class BookViewModel : ViewModelBase
    {
        private readonly Book _book;

        public BookViewModel(Book book)
        {
            _book = book;
        }

        public string Name
        {
            get { return _book.Name!; }
            set
            {
                _book.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int IdAuthor
        {
            get { return _book.IdAuthor; }
        }
    }
}