using Library;
using System.Windows;
namespace Library
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Musisz połączyć tę metodę z przyciskiem "Continue" w MainWindow.xaml
        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            // Otwórz okno logowania
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            // Zamknij okno powitalne
            this.Close();
        }
    }
}