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

namespace Library.Views.Librarian
{
    /// <summary>
    /// Interaction logic for RentalsPage.xaml
    /// </summary>
    public partial class RentalsPage : UserControl
    {
        public RentalsPage()
        {
            InitializeComponent();
            // dwa przyciski filtrowania: 1.Wypożyczenia użytkowników 2.Wypożyczenia książek
            // wyświetlić liste użytkowników po prawej stronie count ich wypożyczeń
            // po kliknięciu w użytkownika przekieroweuje do strony z historią wypożyczeń danego użytkownika
            // wyświetlić list książek po prawej stronie count ich wypożyczeń
            // po kliknięciu w książke przekierowuje do strony z historią wypożyczeń danej książki
        }
    }
}
