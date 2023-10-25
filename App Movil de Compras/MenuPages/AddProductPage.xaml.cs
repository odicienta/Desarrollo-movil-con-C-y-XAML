using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppBanka.Model;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Linq;
using System.Collections.ObjectModel;

namespace AppBanka.MenuPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProductPage : ContentPage
    {
        private ShoppingList SelectedShoppingList { get; set; }
        private List<ShoppingList> ShoppingLists { get; set; }
        private ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

        public AddProductPage(ShoppingList shoppingList, List<ShoppingList> shoppingLists)
        {
            InitializeComponent();
            LoadStoredProducts();
            SelectedShoppingList = shoppingList ?? new ShoppingList();
            ShoppingLists = shoppingLists; // Set the shopping lists
            productsListView.ItemsSource = Products;
        }

        private void LoadStoredProducts()
        {
            string serializedProducts = Preferences.Get("Products", string.Empty);
            if (!string.IsNullOrWhiteSpace(serializedProducts))
            {
                List<Product> storedProducts = JsonConvert.DeserializeObject<List<Product>>(serializedProducts);
                foreach (var storedProduct in storedProducts)
                {
                    Products.Add(storedProduct);
                }
            }
        }

        private void SaveProducts()
        {
            string serializedProducts = JsonConvert.SerializeObject(Products.ToList());
            Preferences.Set("Products", serializedProducts);
        }

        private void AddProductButton_Clicked(object sender, EventArgs e)
        {
            string productName = productNameEntry.Text;
            if (!string.IsNullOrWhiteSpace(productName))
            {
                Products.Add(new Product { Name = productName });
                productNameEntry.Text = string.Empty; // Clear entry

                SaveProducts(); // Save the products after adding
            }
        }

        private void OnProductCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var stackLayout = (StackLayout)checkBox.Parent;
            var label = stackLayout.Children.OfType<Label>().FirstOrDefault();

            if (label != null)
            {
                label.IsEnabled = !checkBox.IsChecked;
            }
        }

        private async void EditProductButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var product = (Product)button.BindingContext;

            if (product != null)
            {
                string newProductName = await DisplayPromptAsync("Edit Product", "Enter new product name:", "OK", "Cancel", product.Name);

                if (!string.IsNullOrWhiteSpace(newProductName))
                {
                    product.Name = newProductName;
                    SaveProducts(); // Save the products after editing
                }
            }
        }

        private void DeleteProductButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var product = (Product)button.BindingContext;

            if (product != null)
            {
                Products.Remove(product);
                SaveProducts(); // Save the products after deleting
            }
        }

        private void UpdateShoppingLists()
        {
            // Find the index of the selected shopping list
            int selectedIndex = ShoppingLists.FindIndex(list => list.Name == SelectedShoppingList.Name);

            if (selectedIndex != -1)
            {
                ShoppingLists[selectedIndex] = SelectedShoppingList;
                SaveShoppingLists();
            }
        }

        private void SaveShoppingLists()
        {
            string serializedLists = JsonConvert.SerializeObject(ShoppingLists);
            Preferences.Set("ShoppingLists", serializedLists);
        }
    }
}
