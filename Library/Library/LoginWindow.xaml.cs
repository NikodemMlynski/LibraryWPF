using Library.Models; // Dodaj referencję do modeli, jeśli potrzebna w UI
using Library.Services;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Library
{
    public partial class LoginWindow : Window
    {
        // Prywatne pola do przechowywania danych wejściowych z UI
        private bool isRegistering = false;
        private string password;
        private string repeatPassword;
        private string name;
        private string email;

        // Serwis użytkowników do interakcji z bazą danych
        private readonly UserService _userService = new UserService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void RegisterText_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isRegistering = true;
            LoginPanel.Visibility = Visibility.Collapsed;
            RegisterPanel.Visibility = Visibility.Visible;
            PasswordError.Text = "";
            LoginError.Text = "";
        }

        private void BackToLogin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isRegistering = false;
            RegisterPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
            PasswordError.Text = "";
            LoginError.Text = "";
        }

        private bool Validate_Inputs()
        {
            // Walidacja dla rejestracji
            string passwordTxt = RegisterPassword.Password;
            string repeatPasswordTxt = RegisterRepeatPassword.Password;
            string emailTxt = RegisterEmail.Text;
            string nameTxt = RegisterName.Text;

            // Użycie PasswordError do wyświetlania błędów rejestracji

            if (string.IsNullOrEmpty(passwordTxt) || string.IsNullOrEmpty(repeatPasswordTxt) || string.IsNullOrEmpty(emailTxt) || string.IsNullOrEmpty(nameTxt))
            {
                PasswordError.Text = "Please fill all fields";
                return false;
            }
            if (passwordTxt.Length < 4 || repeatPasswordTxt.Length < 4)
            {
                PasswordError.Text = "Passwords should have at least 4 characters.";
                return false;
            }
            if (passwordTxt != repeatPasswordTxt)
            {
                PasswordError.Text = "Passwords do not match.";
                return false;
            }
            if (!emailTxt.Contains("@") || emailTxt.Length < 4)
            {
                PasswordError.Text = "Invalid Email";
                return false;
            }

            PasswordError.Text = "";

            password = passwordTxt;
            repeatPassword = repeatPasswordTxt;
            email = emailTxt;
            name = nameTxt;
            return true;
        }

        // --- REJESTRACJA ---
        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate_Inputs()) return;

            // Walidacja i próba rejestracji
            bool success = await _userService.RegisterUserAsync(name, email, password);
            if (success)
            {
                MessageBox.Show("Account created successfully! You can now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                // Wyczyść pola
                RegisterPassword.Password = "";
                RegisterRepeatPassword.Password = "";
                // Wróć do ekranu logowania
                BackToLogin_MouseLeftButtonUp(sender, null);
            }
            else
            {
                PasswordError.Text = "User with this email already exists.";
            }
        }

        // --- LOGOWANIE ---
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string emailTxt = LoginEmail.Text;
            string passwordTxt = LoginPassword.Password;

            if (string.IsNullOrEmpty(emailTxt) || string.IsNullOrEmpty(passwordTxt))
            {
                LoginError.Text = "Please enter email and password.";
                return;
            }

            // Próba zalogowania i pobranie obiektu User
            var user = await _userService.LoginUserAsync(emailTxt, passwordTxt);

            if (user != null)
            {
                LoginError.Text = "";

                // Zapisz zalogowanego użytkownika w globalnym serwisie autoryzacji
                AuthService.Instance.SetUser(user);

                // Otwórz DashboardWindow
                var dashboardWindow = new DashboardWindow();
                dashboardWindow.Show();
                this.Close(); // Zamknij okno logowania
            }
            else
            {
                LoginError.Text = "Invalid credentials";
            }
        }
    }
}