using ADO.NET_HW11.Commands;
using ADO.NET_HW11.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace ADO.NET_HW11.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<AuthorViewModel> AuthorsList { get; set; }
        private ObservableCollection<BookViewModel> _booksByAuthor;

        public ObservableCollection<BookViewModel> BooksList { get; set; }

        public MainViewModel(IQueryable<Author> authors, IQueryable<Book> books)
        {
            AuthorsList = new ObservableCollection<AuthorViewModel>(authors.Select(a => new AuthorViewModel(a)));
            BooksList = new ObservableCollection<BookViewModel>(books.Select(b => new BookViewModel(b)));
        }

        private string _authorFirstName;

        public string AuthorFirstName
        {
            get
            {
                return _authorFirstName;
            }
            set
            {
                _authorFirstName = value;
                OnPropertyChanged(nameof(AuthorFirstName));
            }
        }

        private string _authorLastName;

        public string AuthorLastName
        {
            get
            {
                return _authorLastName;
            }
            set
            {
                _authorLastName = value;
                OnPropertyChanged(nameof(AuthorLastName));
            }
        }

        private int _indexSelectedAuthors = -1;

        public int IndexSelectedAuthors
        {
            get { return _indexSelectedAuthors; }
            set
            {
                _indexSelectedAuthors = value;
                OnPropertyChanged(nameof(IndexSelectedAuthors));
            }
        }

        private string _bookName;

        public string BookName
        {
            get
            {
                return _bookName;
            }
            set
            {
                _bookName = value;
                OnPropertyChanged(nameof(BookName));
            }
        }

        private int _idAuthor;

        public int IdAuthor
        {
            get { return _idAuthor; }
            set
            {
                _idAuthor = value;
                OnPropertyChanged(nameof(IdAuthor));
            }
        }

        private int _indexSelectedBooks = -1;

        public int IndexSelectedBooks
        {
            get { return _indexSelectedBooks; }
            set
            {
                _indexSelectedBooks = value;
                OnPropertyChanged(nameof(IndexSelectedBooks));
            }
        }

        private bool _filterByAuthor;
        public bool FilterByAuthor
        {
            get { return _filterByAuthor; }
            set
            {
                _filterByAuthor = value;
                OnPropertyChanged(nameof(FilterByAuthor));
            }
        }

        private void FilterBooksByAuthor()
        {
            BooksList = new ObservableCollection<BookViewModel>(_booksByAuthor);
            if (FilterByAuthor)
            {
                BooksList = new ObservableCollection<BookViewModel>(_booksByAuthor.Where(b => b.IdAuthor == IndexSelectedAuthors + 1));
            }
            else
            {
                BooksList = new ObservableCollection<BookViewModel>(_booksByAuthor);
            }
        }

        private DelegateCommand _addAuthorCommand;

        public ICommand AddAuthorCommand
        {
            get
            {
                if (_addAuthorCommand == null)
                {
                    _addAuthorCommand = new DelegateCommand(param => AddAuthor(), param => CanAddAuthor());
                }
                return _addAuthorCommand;
            }
        }

        private DelegateCommand _deleteAuthorCommand;

        public ICommand DeleteAuthorCommand
        {
            get
            {
                if (_deleteAuthorCommand == null)
                {
                    _deleteAuthorCommand = new DelegateCommand(param => DeleteAuthor(), param => CanDeleteAuthor());
                }
                return _deleteAuthorCommand;
            }
        }

        private DelegateCommand _updateAuthorCommand;

        public ICommand UpdateAuthorCommand
        {
            get
            {
                if (_updateAuthorCommand == null)
                {
                    _updateAuthorCommand = new DelegateCommand(param => UpdateAuthor(), param => CanUpdateAuthor());
                }
                return _updateAuthorCommand;
            }
        }

        private void AddAuthor()
        {
            try
            {
                using (var db = new AuthorsAndBooksContext())
                {
                    var author = new Author { FirstName = AuthorFirstName };
                    db.Authors.Add(author);
                    db.SaveChanges();
                    var authorViewModel = new AuthorViewModel(author);
                    AuthorsList.Add(authorViewModel);
                    MessageBox.Show("Автора додано успішно");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanAddAuthor()
        {
            return !AuthorFirstName.IsNullOrEmpty() && !AuthorLastName.IsNullOrEmpty();
        }

        private void DeleteAuthor()
        {
            try
            {
                var deleteAuthor = AuthorsList[IndexSelectedAuthors];
                DialogResult result = MessageBox.Show($"Ви дійсно хочете видалити автора {deleteAuthor.FirstName} {deleteAuthor.LastName}?", "Видалення автора", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    return;
                using (var db = new AuthorsAndBooksContext())
                {
                    var query = (from a in db.Authors
                                 where a.FirstName == deleteAuthor.FirstName 
                                 && a.LastName == deleteAuthor.LastName
                                 select a).Single();
                    db.Authors.Remove(query);
                    db.SaveChanges();
                    AuthorsList.Remove(deleteAuthor);
                    MessageBox.Show("Автора видалено успішно");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanDeleteAuthor()
        {
            return IndexSelectedAuthors != -1;
        }

        private void UpdateAuthor()
        {
            try
            {
                using (var db = new AuthorsAndBooksContext())
                {
                    var updateAuthor = AuthorsList[IndexSelectedAuthors];
                    var query = (from a in db.Authors
                                 where a.FirstName == updateAuthor.FirstName 
                                 && a.LastName == updateAuthor.LastName
                                 select a).Single();
                    query.FirstName = AuthorFirstName;
                    query.LastName = AuthorLastName;
                    db.SaveChanges();
                    AuthorsList[IndexSelectedAuthors] = new AuthorViewModel(query);
                    MessageBox.Show("Автора оновлено успішно");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanUpdateAuthor()
        {
            return (!AuthorFirstName.IsNullOrEmpty() || !AuthorLastName.IsNullOrEmpty()) && IndexSelectedAuthors != -1;
        }

        private DelegateCommand _addBookCommand;

        public ICommand _AddBookCommand
        {
            get
            {
                if (_addBookCommand == null)
                {
                    _addBookCommand = new DelegateCommand(param => AddBook(), param => CanAddBook());
                }
                return _addBookCommand;
            }
        }

        private void AddBook()
        {
            try
            {
                using (var db = new AuthorsAndBooksContext())
                {
                    var author = AuthorsList[IndexSelectedAuthors];
                    var query = (from a in db.Authors
                                 where a.FirstName == author.FirstName 
                                 && a.LastName == author.LastName
                                 select a).Single();

                    var book = new Book
                    {
                        Name = BookName,
                        Author = query
                    };
                    db.Books.Add(book);
                    db.SaveChanges();
                    var bookViewModel = new BookViewModel(book);
                    BooksList.Add(bookViewModel);

                    MessageBox.Show("Книгу додано успішно");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanAddBook()
        {
            return !BookName.IsNullOrEmpty() && IndexSelectedAuthors != -1;

        }

        private DelegateCommand _deleteBookCommand;

        public ICommand DeleteBookCommand
        {
            get
            {
                if (_deleteBookCommand == null)
                {
                    _deleteBookCommand = new DelegateCommand(param => DeleteBook(), param => CanDeleteBook());
                }
                return _deleteBookCommand;
            }
        }

        private void DeleteBook()
        {
            try
            {
                var deleteBook = BooksList[IndexSelectedBooks];
                DialogResult result = MessageBox.Show($"Ви дійсно хочете видалити книгу \"{deleteBook.Name}\"?", "Видалення книги", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    return;
                using (var db = new AuthorsAndBooksContext())
                {
                    var query = from b in db.Books
                                where b.Name == deleteBook.Name
                                select b;
                    db.Books.RemoveRange(query);
                    db.SaveChanges();
                    BooksList.Remove(deleteBook);
                    MessageBox.Show("Книгу видалено успішно");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanDeleteBook()
        {
            return IndexSelectedBooks != -1;
        }

        private DelegateCommand _updateBookCommand;

        public ICommand UpdateBookCommand
        {
            get
            {
                if (_updateBookCommand == null)
                {
                    _updateBookCommand = new DelegateCommand(param => UpdateBook(), param => CanUpdateBook());
                }
                return _updateBookCommand;
            }
        }

        private void UpdateBook()
        {
            try
            {
                using (var db = new AuthorsAndBooksContext())
                {
                    var author = AuthorsList[IndexSelectedAuthors];
                    var bookAuthor = (from a in db.Authors
                                 where a.FirstName == author.FirstName
                                 && a.LastName == author.LastName
                                 select a).Single();
                    var updateBook = BooksList[IndexSelectedBooks];
                    if (bookAuthor == null)
                        return;

                    var book = (from b in db.Books
                                   where b.Name == updateBook.Name
                                   select b).Single();

                    book.Author = bookAuthor;
                    book.Name = BookName;
                    db.SaveChanges();
                    BooksList[IndexSelectedBooks] = new BookViewModel(book);
                    MessageBox.Show("Дані про книгу оновлено успішно");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanUpdateBook()
        {
            return !BookName.IsNullOrEmpty() && IndexSelectedAuthors != -1 && IndexSelectedBooks != -1;
        }
    }
}