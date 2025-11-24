using Library.Models;
using Library.Models;
using System;

namespace Library.Services
{
    // Używamy patternu Singleton, aby mieć tylko jedną instancję tego serwisu w całej aplikacji
    public class AuthService
    {
        private static readonly AuthService _instance = new AuthService();
        public static AuthService Instance => _instance;

        private User _currentUser;

        // Publiczna właściwość przechowująca zalogowanego użytkownika
        public User CurrentUser
        {
            get => _currentUser;
            private set
            {
                _currentUser = value;
                // Możesz dodać zdarzenie, jeśli potrzebujesz odświeżania wielu elementów UI,
                // ale dla prostego zarządzania oknami (Login/Dashboard) nie jest to konieczne.
                OnUserChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler OnUserChanged;

        // Prywatny konstruktor, aby wymusić użycie Singletona
        private AuthService() { }

        // Metoda do ustawienia użytkownika po pomyślnym zalogowaniu
        public void SetUser(User user)
        {
            CurrentUser = user;
        }

        // Metoda wylogowania
        public void Logout()
        {
            CurrentUser = null;
        }

        // Sprawdza, czy jest zalogowany użytkownik
        public bool IsLoggedIn()
        {
            return CurrentUser != null;
        }
    }
}