using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Library.ViewModels.Reader;

namespace Library.Views.Reader
{
    /// <summary>
    /// Interaction logic for BookListPage.xaml
    /// </summary>
    public partial class BookListPage : UserControl
    {
        public BookListPage()
        {
            InitializeComponent();
            //DataContext = new BookListPageViewModel();
        }
    }
}
