using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ElectronicsShop.Models;
using ElectronicsShop.Services;

namespace ElectronicsShop.Pages
{
    public partial class BrandEditPage : Page, INotifyPropertyChanged
    {
        private ElectronicsShopDbContext db = DBService.Instance.Context;
        private Brand? currentBrand;
        public string WindowTitle => currentBrand == null ? "Добавление бренда" : "Редактирование бренда";

        private string brandName = string.Empty;
        public string BrandName
        {
            get => brandName;
            set
            {
                brandName = value;
                OnPropertyChanged(nameof(BrandName));
            }
        }

        private string brandCountry = string.Empty;
        public string BrandCountry
        {
            get => brandCountry;
            set
            {
                brandCountry = value;
                OnPropertyChanged(nameof(BrandCountry));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BrandEditPage(Brand? brand)
        {
            currentBrand = brand;
            InitializeComponent();
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (currentBrand != null)
            {
                BrandName = currentBrand.Name;
                BrandCountry = currentBrand.Country ?? string.Empty;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в форме", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (currentBrand == null)
                {
                    var newBrand = new Brand
                    {
                        Name = BrandName.Trim(),
                        Country = BrandCountry.Trim()
                    };

                    db.Brands.Add(newBrand);
                    db.SaveChanges();
                    MessageBox.Show("Бренд успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    currentBrand.Name = BrandName.Trim();
                    currentBrand.Country = BrandCountry.Trim();
                    db.SaveChanges();
                    MessageBox.Show("Бренд успешно обновлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                if (NavigationService?.CanGoBack == true)
                    NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении бренда: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(BrandName) || BrandName.Length > 100)
                return false;

            if (string.IsNullOrWhiteSpace(BrandCountry) || BrandCountry.Length > 100)
                return false;

            return true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
        }
    }
}