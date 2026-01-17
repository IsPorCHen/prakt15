using System.Windows;
using ElectronicsShop.Pages;

namespace ElectronicsShop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new LoginPage());
        }
    }
}