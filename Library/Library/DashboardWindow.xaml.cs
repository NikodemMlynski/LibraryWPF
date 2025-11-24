using Library.ViewModels;
using System.Windows;

namespace Library
{
    /// <summary>
    /// Logika interakcji dla DashboardWindow.xaml.
    /// To jest "shell" aplikacji, który hostuje dynamiczne widoki.
    /// </summary>
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();

            // Ustawienie głównego ViewModelu jako kontekstu danych dla okna.
            // Od tego momentu wszystkie bindowania w XAML działają na DashboardViewModel.
            DataContext = new DashboardViewModel();
    }
    }
}