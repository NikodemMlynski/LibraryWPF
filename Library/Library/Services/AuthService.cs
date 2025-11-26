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

        public object CurrentIdentity { get; private set; }

        public UserRole CurrentRole { get; private set; } = UserRole.None;

        public User CurrentUser => CurrentIdentity as User;
        public Admin CurrentAdmin => CurrentIdentity as Admin;
        public Librarian CurrentLibrarian => CurrentIdentity as Librarian;

        private AuthService() { }

        public void SetSession(object identity, UserRole role)
        {
            CurrentIdentity = identity;
            CurrentRole = role;
        }

        public void Logout()
        {
            CurrentIdentity = null;
            CurrentRole = UserRole.None;
        }

        public bool IsLoggedIn()
        {
            return CurrentIdentity != null;
        }

    }
}