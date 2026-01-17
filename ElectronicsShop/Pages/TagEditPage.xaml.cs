using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ElectronicsShop.Models;
using ElectronicsShop.Services;

namespace ElectronicsShop.Pages
{
    public partial class TagEditPage : Page, INotifyPropertyChanged
    {
        private ElectronicsShopDbContext db = DBService.Instance.Context;
        private Tag? currentTag;
        public string WindowTitle => currentTag == null ? "Добавление тега" : "Редактирование тега";

        private string tagName = string.Empty;
        public string TagName
        {
            get => tagName;
            set
            {
                tagName = value;
                OnPropertyChanged(nameof(TagName));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TagEditPage(Tag? tag)
        {
            currentTag = tag;
            InitializeComponent();
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (currentTag != null)
            {
                TagName = currentTag.Name;
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
                if (currentTag == null)
                {
                    var newTag = new Tag
                    {
                        Name = TagName.Trim()
                    };

                    db.Tags.Add(newTag);
                    db.SaveChanges();
                    MessageBox.Show("Тег успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    currentTag.Name = TagName.Trim();
                    db.SaveChanges();
                    MessageBox.Show("Тег успешно обновлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                if (NavigationService?.CanGoBack == true)
                    NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении тега: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(TagName) || TagName.Length > 50)
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