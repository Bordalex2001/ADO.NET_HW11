using ADO.NET_HW11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_HW11.ViewModels
{
    public class AuthorViewModel : ViewModelBase
    {
        private readonly Author _author;

        public AuthorViewModel(Author author)
        {
            _author = author;
        }

        public string FirstName
        {
            get { return _author.FirstName!; }
            set
            {
                _author.FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get { return _author.LastName!; }
            set
            {
                _author.LastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
    }
}