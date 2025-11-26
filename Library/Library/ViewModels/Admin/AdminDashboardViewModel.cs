using Library.Services;
using Library.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
namespace Library.ViewModels.Admin
{
    public class AdminDashboardViewModel : BaseViewModel
    {
        private BaseViewModel _currentPageViewModel;
        private List<BaseViewModel> _pageViewModels;

        public ICommand NavigateCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        public AdminDashboardViewModel()
        {
            NavigateCommand = new RelayCommand(Navigate);
            LogoutCommand = new RelayCommand(Logout);

            // Inicjalizacja dostępnych stron dla Bibliotekarza
            PageViewModels = new List<BaseViewModel>
            {
                new DashboardPageViewModel(),
                new BookManagerPageViewModel(),
                new LibrarianManagerPageViewModel(),
                new ReaderManagerPageViewModel(),
                new RentalsManagerPageViewModel(),
            };

            // Ustawienie strony startowej
            CurrentPageViewModel = PageViewModels[0];
        }

        public List<BaseViewModel> PageViewModels
        {
            get => _pageViewModels;
            set => SetValue(ref _pageViewModels, value);
        }

        public BaseViewModel CurrentPageViewModel
        {
            get => _currentPageViewModel;
            set => SetValue(ref _currentPageViewModel, value);
        }

        private void Navigate(object viewModelType)
        {
            if (viewModelType == null) return;

            // Szukamy odpowiedniego ViewModelu na liście po nazwie typu
            string typeName = viewModelType.ToString();
            var viewModel = PageViewModels.FirstOrDefault(vm => vm.GetType().Name == typeName);

            if (viewModel != null)
            {
                CurrentPageViewModel = viewModel;
            }
        }

        private void Logout(object obj)
        {
            AuthService.Instance.Logout();

            // Logika zamknięcia okna i otwarcia logowania
            // (Można to zrobić czyściej przez serwisy, ale na ten etap wystarczy proste podejście)
            var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            currentWindow?.Close();

            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}