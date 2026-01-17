using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ElectronicsShop.Models;
using ElectronicsShop.Services;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Pages
{
    public partial class ManageDataPage : Page, INotifyPropertyChanged
    {
        private ElectronicsShopDbContext db = DBService.Instance.Context;

        public ObservableCollection<Product> Products { get; set; } = new();
        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<Brand> Brands { get; set; } = new();
        public ObservableCollection<Tag> Tags { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ManageDataPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            Products.Clear();
            Categories.Clear();
            Brands.Clear();
            Tags.Clear();

            foreach (var product in db.Products.Include(p => p.Brand).Include(p => p.Category).ToList())
                Products.Add(product);

            foreach (var category in db.Categories.ToList())
                Categories.Add(category);

            foreach (var brand in db.Brands.ToList())
                Brands.Add(brand);

            foreach (var tag in db.Tags.ToList())
                Tags.Add(tag);
        }

        private void BackToCatalog_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
        }

        // Товары
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ProductEditPage(null));
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Product product)
            {
                NavigationService?.Navigate(new ProductEditPage(product));
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Product product)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить товар '{product.Name}'?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        db.Products.Remove(product);
                        db.SaveChanges();
                        LoadData();
                        MessageBox.Show("Товар успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении товара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Категории
        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CategoryEditPage(null));
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesDataGrid.SelectedItem is Category category)
            {
                NavigationService?.Navigate(new CategoryEditPage(category));
            }
            else
            {
                MessageBox.Show("Выберите категорию для редактирования", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesDataGrid.SelectedItem is Category category)
            {
                var hasProducts = db.Products.Any(p => p.CategoryId == category.Id);
                if (hasProducts)
                {
                    MessageBox.Show("Невозможно удалить категорию, так как к ней привязаны товары", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить категорию '{category.Name}'?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        db.Categories.Remove(category);
                        db.SaveChanges();
                        LoadData();
                        MessageBox.Show("Категория успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении категории: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите категорию для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Бренды
        private void AddBrand_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new BrandEditPage(null));
        }

        private void EditBrand_Click(object sender, RoutedEventArgs e)
        {
            if (BrandsDataGrid.SelectedItem is Brand brand)
            {
                NavigationService?.Navigate(new BrandEditPage(brand));
            }
            else
            {
                MessageBox.Show("Выберите бренд для редактирования", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteBrand_Click(object sender, RoutedEventArgs e)
        {
            if (BrandsDataGrid.SelectedItem is Brand brand)
            {
                var hasProducts = db.Products.Any(p => p.BrandId == brand.Id);
                if (hasProducts)
                {
                    MessageBox.Show("Невозможно удалить бренд, так как к нему привязаны товары", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить бренд '{brand.Name}'?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        db.Brands.Remove(brand);
                        db.SaveChanges();
                        LoadData();
                        MessageBox.Show("Бренд успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении бренда: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите бренд для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Теги
        private void AddTag_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new TagEditPage(null));
        }

        private void EditTag_Click(object sender, RoutedEventArgs e)
        {
            if (TagsDataGrid.SelectedItem is Tag tag)
            {
                NavigationService?.Navigate(new TagEditPage(tag));
            }
            else
            {
                MessageBox.Show("Выберите тег для редактирования", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteTag_Click(object sender, RoutedEventArgs e)
        {
            if (TagsDataGrid.SelectedItem is Tag tag)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить тег '{tag.Name}'?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        db.Tags.Remove(tag);
                        db.SaveChanges();
                        LoadData();
                        MessageBox.Show("Тег успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении тега: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите тег для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}