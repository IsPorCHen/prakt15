using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ElectronicsShop.Models;
using ElectronicsShop.Services;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Pages
{
    public partial class ProductEditPage : Page, INotifyPropertyChanged
    {
        private ElectronicsShopDbContext db = DBService.Instance.Context;
        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<Brand> Brands { get; set; } = new();
        public ObservableCollection<Tag> Tags { get; set; } = new();

        private Product? currentProduct;
        public string WindowTitle => currentProduct == null ? "Добавление товара" : "Редактирование товара";

        private string productName = string.Empty;
        public string ProductName
        {
            get => productName;
            set
            {
                productName = value;
                OnPropertyChanged(nameof(ProductName));
            }
        }

        private string productDescription = string.Empty;
        public string ProductDescription
        {
            get => productDescription;
            set
            {
                productDescription = value;
                OnPropertyChanged(nameof(ProductDescription));
            }
        }

        private string productPrice = string.Empty;
        public string ProductPrice
        {
            get => productPrice;
            set
            {
                productPrice = value;
                OnPropertyChanged(nameof(ProductPrice));
            }
        }

        private string productQuantity = string.Empty;
        public string ProductQuantity
        {
            get => productQuantity;
            set
            {
                productQuantity = value;
                OnPropertyChanged(nameof(ProductQuantity));
            }
        }

        private int? selectedCategoryId;
        public int? SelectedCategoryId
        {
            get => selectedCategoryId;
            set
            {
                selectedCategoryId = value;
                OnPropertyChanged(nameof(SelectedCategoryId));
            }
        }

        private int? selectedBrandId;
        public int? SelectedBrandId
        {
            get => selectedBrandId;
            set
            {
                selectedBrandId = value;
                OnPropertyChanged(nameof(SelectedBrandId));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ProductEditPage(Product? product)
        {
            currentProduct = product;
            InitializeComponent();
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();

            if (currentProduct != null)
            {
                LoadProductData();
            }
        }

        private void LoadData()
        {
            Categories.Clear();
            Brands.Clear();
            Tags.Clear();

            foreach (var category in db.Categories.ToList())
                Categories.Add(category);

            foreach (var brand in db.Brands.ToList())
                Brands.Add(brand);

            foreach (var tag in db.Tags.ToList())
                Tags.Add(tag);
        }

        private void LoadProductData()
        {
            if (currentProduct == null) return;

            db.Entry(currentProduct)
                .Reference(p => p.Category)
                .Load();

            db.Entry(currentProduct)
                .Reference(p => p.Brand)
                .Load();

            db.Entry(currentProduct)
                .Collection(p => p.Tags)
                .Load();

            ProductName = currentProduct.Name;
            ProductDescription = currentProduct.Description ?? string.Empty;
            ProductPrice = currentProduct.Price.ToString("F2");
            ProductQuantity = currentProduct.Quantity.ToString();
            SelectedCategoryId = currentProduct.CategoryId;
            SelectedBrandId = currentProduct.BrandId;

            foreach (var tag in Tags)
            {
                if (currentProduct.Tags.Any(t => t.Id == tag.Id))
                {
                    TagsListBox.SelectedItems.Add(tag);
                }
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
                if (currentProduct == null)
                {
                    var newProduct = new Product
                    {
                        Name = ProductName.Trim(),
                        Description = string.IsNullOrWhiteSpace(ProductDescription) ? null : ProductDescription.Trim(),
                        Price = decimal.Parse(ProductPrice.Trim()),
                        Quantity = int.Parse(ProductQuantity.Trim()),
                        CategoryId = SelectedCategoryId.Value,
                        BrandId = SelectedBrandId.Value
                    };

                    foreach (Tag tag in TagsListBox.SelectedItems)
                    {
                        var existingTag = db.Tags.Find(tag.Id);
                        if (existingTag != null)
                        {
                            newProduct.Tags.Add(existingTag);
                        }
                    }

                    db.Products.Add(newProduct);
                    db.SaveChanges();

                    MessageBox.Show("Товар успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    db.Entry(currentProduct)
                        .Collection(p => p.Tags)
                        .Load();

                    currentProduct.Name = ProductName.Trim();
                    currentProduct.Description = string.IsNullOrWhiteSpace(ProductDescription) ? null : ProductDescription.Trim();
                    currentProduct.Price = decimal.Parse(ProductPrice.Trim());
                    currentProduct.Quantity = int.Parse(ProductQuantity.Trim());
                    currentProduct.CategoryId = SelectedCategoryId.Value;
                    currentProduct.BrandId = SelectedBrandId.Value;

                    currentProduct.Tags.Clear();

                    foreach (Tag tag in TagsListBox.SelectedItems)
                    {
                        var existingTag = db.Tags.Find(tag.Id);
                        if (existingTag != null)
                        {
                            currentProduct.Tags.Add(existingTag);
                        }
                    }

                    db.SaveChanges();
                    MessageBox.Show("Товар успешно обновлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                if (NavigationService?.CanGoBack == true)
                    NavigationService.GoBack();
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show($"Ошибка базы данных при сохранении товара: {ex.InnerException?.Message ?? ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении товара: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private bool ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(ProductName) || ProductName.Length > 200)
                isValid = false;

            if (ProductDescription.Length > 1000)
                isValid = false;

            if (string.IsNullOrWhiteSpace(ProductPrice) || !decimal.TryParse(ProductPrice, out decimal price) || price <= 0)
                isValid = false;

            if (string.IsNullOrWhiteSpace(ProductQuantity) || !int.TryParse(ProductQuantity, out int qty) || qty < 0)
                isValid = false;

            if (!SelectedCategoryId.HasValue)
                isValid = false;

            if (!SelectedBrandId.HasValue)
                isValid = false;

            return isValid;
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
        }

        private void DecimalValidation_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            var fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            Regex regex = new Regex(@"^\d*\.?\d*$");
            e.Handled = !regex.IsMatch(fullText);
        }

        private void IntegerValidation_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void PriceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && decimal.TryParse(textBox.Text, out decimal price))
            {
                textBox.Text = price.ToString("F2");
            }
        }
    }
}