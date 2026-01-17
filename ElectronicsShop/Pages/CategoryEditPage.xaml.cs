using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ElectronicsShop.Models;
using ElectronicsShop.Services;

namespace ElectronicsShop.Pages
{
    public partial class CategoryEditPage : Page, INotifyPropertyChanged
    {
        private ElectronicsShopDbContext db = DBService.Instance.Context;
        private Category? currentCategory;
        public string WindowTitle => currentCategory == null ? "Добавление категории" : "Редактирование категории";

        private string categoryName = string.Empty;
        public string CategoryName
        {
            get => categoryName;
            set
            {
                categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }

        private string categoryDescription = string.Empty;
        public string CategoryDescription
        {
            get => categoryDescription;
            set
            {
                categoryDescription = value;
                OnPropertyChanged(nameof(CategoryDescription));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CategoryEditPage(Category? category)
        {
            currentCategory = category;
            InitializeComponent();
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (currentCategory != null)
            {
                CategoryName = currentCategory.Name;
                CategoryDescription = currentCategory.Description ?? string.Empty;
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
                if (currentCategory == null)
                {
                    var newCategory = new Category
                    {
                        Name = CategoryName.Trim(),
                        Description = CategoryDescription.Trim()
                    };

                    db.Categories.Add(newCategory);
                    db.SaveChanges();
                    MessageBox.Show("Категория успешно добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    currentCategory.Name = CategoryName.Trim();
                    currentCategory.Description = CategoryDescription.Trim();
                    db.SaveChanges();
                    MessageBox.Show("Категория успешно обновлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                if (NavigationService?.CanGoBack == true)
                    NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении категории: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(CategoryName) || CategoryName.Length > 100)
                return false;

            if (CategoryDescription.Length > 500)
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