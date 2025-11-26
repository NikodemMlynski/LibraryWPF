using Library;
using Library.Models;
using Library.Services;
using Library.ViewModels;
using Library.ViewModels.Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows; // Wymagane dla Application
using System.Windows.Input;

namespace Library.ViewModels.Reader
{
    public class DashboardViewModel : BaseViewModel
    {
        // Prywatna zmienna przechowująca aktualnie wyświetlany ViewModel strony
        private BaseViewModel _currentPageViewModel;

        // Lista wszystkich dostępnych ViewModeli stron
        private List<BaseViewModel> _pageViewModels;
        public User CurrentUser => AuthService.Instance.CurrentUser;

        // Komendy
        public ICommand NavigateCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; } // Nowa komenda

        // Właściwość do pobierania aktualnie zalogowanego użytkownika z globalnego serwisu
        // (Ważne: W BaseViewModel musi być zaimplementowany mechanizm INotifyPropertyChanged)

        public DashboardViewModel()
        {
            // Inicjalizacja komend
            NavigateCommand = new RelayCommand(Navigate);
            LogoutCommand = new RelayCommand(Logout);

            // Dodaj wszystkie ViewModele stron do listy
            PageViewModels = new List<BaseViewModel>
            {
                new DashboardPageViewModel(),
                new BookListPageViewModel(),
                new UserRentalsPageViewModel(),
                new UserProfilePageViewModel()
            };

            // Ustaw początkowy ViewModel (Dashboard)
            CurrentPageViewModel = PageViewModels.First(vm => vm is DashboardPageViewModel);
        }

        // Publiczna lista ViewModeli
        public List<BaseViewModel> PageViewModels
        {
            get => _pageViewModels;
            set => SetValue(ref _pageViewModels, value);
        }

        // Właściwość, do której będzie bindowany ContentControl
        public BaseViewModel CurrentPageViewModel
        {
            get => _currentPageViewModel;
            set => SetValue(ref _currentPageViewModel, value);
        }

        // Metoda routingu
        private void Navigate(object viewModelTypeName)
        {
            if (viewModelTypeName == null) return;

            string typeName = viewModelTypeName.ToString();
            BaseViewModel viewModel = PageViewModels
                .FirstOrDefault(vm => vm.GetType().Name == typeName);

            if (viewModel != null)
            {
                CurrentPageViewModel = viewModel;
            }
        }

        private void Logout(object obj)
        {
            AuthService.Instance.Logout();

            Window dashboardWindow = Application.Current.Windows.OfType<DashboardWindow>()
                                     .FirstOrDefault(window => window.DataContext == this);

            dashboardWindow?.Close();

            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}