using ADO.NET_HW11.Models;
using ADO.NET_HW11.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ADO.NET_HW11
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            //try
            //{
                using (AuthorsAndBooksContext? db = new())
                {
                    var authors = from a in db.Authors
                                  select a;
                    var books = from b in db.Books
                                select b;
                    MainWindow view = new();
                    MainViewModel viewModel = new(authors, books);
                    view.DataContext = viewModel;
                    view.Show();
                }
            //}
            //catch (Exception ex)
            //{
                //MessageBox.Show(ex.Message);
            //}
        }
    }
}