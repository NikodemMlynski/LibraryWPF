namespace Library.ViewModels
{
    // 1. Dashboard Page ViewModel
    public class DashboardPageViewModel : BaseViewModel, IPageViewModel
    {
        public string Title => "DASHBOARD";
        // Tutaj można dodać logikę specyficzną dla pulpitu (np. statystyki)
    }

    // 2. Book List Page ViewModel
    public class BookListPageViewModel : BaseViewModel, IPageViewModel
    {
        public string Title => "LISTA KSIĄŻEK";
        // Tutaj można dodać logikę pobierania listy książek
    }

    // 3. User Rentals Page ViewModel
    public class UserRentalsPageViewModel : BaseViewModel, IPageViewModel
    {
        public string Title => "TWOJE WYPOŻYCZENIA";
        // Tutaj można dodać logikę pobierania wypożyczeń użytkownika
    }

    // 4. User Profile Page ViewModel
    public class UserProfilePageViewModel : BaseViewModel, IPageViewModel
    {
        public string Title => "PROFIL UŻYTKOWNIKA";
        // Tutaj można dodać logikę zarządzania profilem
    }
}