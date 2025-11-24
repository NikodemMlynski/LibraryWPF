using System.Windows;
using System.Windows.Input;

namespace LibraryApp
{
    public partial class LoginWindow : Window
    {
        private bool isRegistering = false;
        private string password;
        private string repeatPassword;
        private string name;
        private string email;
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
        }

        private void BackToLogin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isRegistering = false;
            RegisterPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
            PasswordError.Text = "";
        }
        private bool Validate_Inputs()
        {
            string passwordTxt = RegisterPassword.Password;
            string repeatPasswordTxt = RegisterRepeatPassword.Password;
            string emailTxt = RegisterEmail.Text;
            string nameTxt = RegisterName.Text;

            if (string.IsNullOrEmpty(passwordTxt) || string.IsNullOrEmpty(repeatPasswordTxt) || string.IsNullOrEmpty(emailTxt) || string.IsNullOrEmpty(nameTxt))
            {
                PasswordError.Text = "Please fill all fields";
                return false;
            }
            if (passwordTxt.Length < 4 || repeatPasswordTxt.Length < 4)
            {
                PasswordError.Text = "Passwords shold have at leat 4 characters.";
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

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string password = RegisterPassword.Password;
            string repeat = RegisterRepeatPassword.Password;

            if (!Validate_Inputs()) return;


            MessageBox.Show("Account created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            BackToLogin_MouseLeftButtonUp(sender, null);
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string emailTxt = LoginEmail.Text;
            string passwordTxt = LoginPassword.Password;

            if (!(emailTxt == "asdf@asdf.pl" && passwordTxt == "asdf1234"))
            {
                LoginError.Text = "Invalid credentials";
                return;
            }
            LoginError.Text = "";
            MessageBox.Show("Successfully logged In!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
