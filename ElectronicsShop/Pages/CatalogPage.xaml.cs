using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ElectronicsShop.Models;
using ElectronicsShop.Services;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Pages
{
    public partial class CatalogPage : Page, INotifyPropertyChanged
    {
        private ElectronicsShopDbContext db = DBService.Instance.Context;
        public ObservableCollection<Product> Products { get; set; } = new();
        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<Brand> Brands { get; set; } = new();
        public ICollectionView ProductsView { get; set; }

        private bool isManager;
        public Visibility ManagerButtonsVisibility => isManager ? Visibility.Visible : Visibility.Collapsed;

        private string searchQuery = string.Empty;
        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        private string priceFrom = string.Empty;
        public string PriceFrom
        {
            get => priceFrom;
            set
            {
                priceFrom = value;
                OnPropertyChanged(nameof(PriceFrom));
            }
        }

        private string priceTo = string.Empty;
        public string PriceTo
        {
            get => priceTo;
            set
            {
                priceTo = value;
                OnPropertyChanged(nameof(PriceTo));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CatalogPage(bool isManager)
        {
            this.isManager = isManager;
            ProductsView = CollectionViewSource.GetDefaultView(Products);
            ProductsView.Filter = FilterProducts;
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

            var products = db.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToList();

            foreach (var product in products)
            {
                Products.Add(product);
            }

            var allCategory = new Category { Id = 0, Name = "Все категории" };
            Categories.Add(allCategory);
            foreach (var category in db.Categories.ToList())
            {
                Categories.Add(category);
            }
            CategoryFilterComboBox.SelectedIndex = 0;

            var allBrand = new Brand { Id = 0, Name = "Все бренды" };
            Brands.Add(allBrand);
            foreach (var brand in db.Brands.ToList())
            {
                Brands.Add(brand);
            }
            BrandFilterComboBox.SelectedIndex = 0;

            ProductsView.Refresh();
        }

        private bool FilterProducts(object obj)
        {
            if (obj is not Product product)
                return false;

            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                bool matchesSearch = product.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                                    (product.Brand?.Name?.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ?? false);
                if (!matchesSearch)
                    return false;
            }

            if (CategoryFilterComboBox.SelectedValue != null)
            {
                int categoryId = (int)CategoryFilterComboBox.SelectedValue;
                if (categoryId != 0 && product.CategoryId != categoryId)
                    return false;
            }

            if (BrandFilterComboBox.SelectedValue != null)
            {
                int brandId = (int)BrandFilterComboBox.SelectedValue;
                if (brandId != 0 && product.BrandId != brandId)
                    return false;
            }

            if (!string.IsNullOrWhiteSpace(PriceFrom))
            {
                if (decimal.TryParse(PriceFrom, out decimal minPrice))
                {
                    if (product.Price < minPrice)
                        return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(PriceTo))
            {
                if (decimal.TryParse(PriceTo, out decimal maxPrice))
                {
                    if (product.Price > maxPrice)
                        return false;
                }
            }

            return true;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ProductsView.Refresh();
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ProductsView.Refresh();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsView != null)
                ProductsView.Refresh();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsView == null || SortComboBox.SelectedItem == null)
                return;

            ProductsView.SortDescriptions.Clear();
            var selected = (ComboBoxItem)SortComboBox.SelectedItem;

            switch (selected.Tag)
            {
                case "Name":
                    ProductsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    break;
                case "PriceAsc":
                    ProductsView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Ascending));
                    break;
                case "PriceDesc":
                    ProductsView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Descending));
                    break;
                case "QuantityAsc":
                    ProductsView.SortDescriptions.Add(new SortDescription("Quantity", ListSortDirection.Ascending));
                    break;
                case "QuantityDesc":
                    ProductsView.SortDescriptions.Add(new SortDescription("Quantity", ListSortDirection.Descending));
                    break;
            }

            ProductsView.Refresh();
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchQuery = string.Empty;
            PriceFrom = string.Empty;
            PriceTo = string.Empty;
            CategoryFilterComboBox.SelectedIndex = 0;
            BrandFilterComboBox.SelectedIndex = 0;
            SortComboBox.SelectedIndex = -1;
            ProductsView.SortDescriptions.Clear();
            ProductsView.Refresh();
        }

        private void NumberValidation_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ManageData_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ManageDataPage());
        }
    }
}