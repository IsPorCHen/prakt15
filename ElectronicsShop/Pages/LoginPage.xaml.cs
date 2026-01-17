using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElectronicsShop.Pages
{
    public partial class LoginPage : Page
    {
        private const string ManagerPin = "1234";

        public LoginPage()
        {
            InitializeComponent();
        }

        private void ManagerLogin_Click(object sender, RoutedEventArgs e)
        {
            if (PinCodeBox.Password == ManagerPin)
            {
                NavigationService?.Navigate(new CatalogPage(isManager: true));
            }
            else
            {
                MessageBox.Show(
                    "Неверный PIN-код! Попробуйте еще раз.",
                    "Ошибка входа",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                PinCodeBox.Clear();
                PinCodeBox.Focus();
            }
        }

        private void VisitorLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CatalogPage(isManager: false));
        }

        private void PinCodeBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ManagerLogin_Click(sender, e);
            }
        }
    }
}