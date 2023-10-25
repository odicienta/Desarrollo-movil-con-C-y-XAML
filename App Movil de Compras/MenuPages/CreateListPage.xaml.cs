using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppBanka.Model;
using Newtonsoft.Json;
using Xamarin.Essentials;
using AppBanka.MenuPages;

namespace AppBanka
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateListPage : ContentPage
    {
        private List<ShoppingList> ShoppingLists { get; set; }

        public CreateListPage()
        {
            InitializeComponent();
            LoadShoppingLists();
            shoppingListsListView.ItemsSource = ShoppingLists;
        }

        private void LoadShoppingLists()
        {
            string serializedLists = Preferences.Get("ShoppingLists", string.Empty);
            if (!string.IsNullOrEmpty(serializedLists))
            {
                ShoppingLists = JsonConvert.DeserializeObject<List<ShoppingList>>(serializedLists);
            }
            else
            {
                ShoppingLists = new List<ShoppingList>();
            }
        }

        private void SaveShoppingLists()
        {
            string serializedLists = JsonConvert.SerializeObject(ShoppingLists);
            Preferences.Set("ShoppingLists", serializedLists);
        }

        private void SaveListButton_Clicked(object sender, EventArgs e)
        {
            string listName = listNameEntry.Text;
            if (!string.IsNullOrWhiteSpace(listName))
            {
                ShoppingLists.Add(new ShoppingList { Name = listName });
                SaveShoppingLists();
                shoppingListsListView.ItemsSource = null; // Clear binding
                shoppingListsListView.ItemsSource = ShoppingLists; // Rebind
            }
        }

        private async void EditListButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button editButton && editButton.CommandParameter is ShoppingList selectedList)
            {
                string newName = await DisplayPromptAsync("Editar Lista", "Ingrese el nuevo nombre:", initialValue: selectedList.Name);
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    selectedList.Name = newName;
                    SaveShoppingLists();
                    shoppingListsListView.ItemsSource = null; // Clear binding
                    shoppingListsListView.ItemsSource = ShoppingLists; // Rebind
                }
            }
        }

        private async void DeleteListButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.CommandParameter is ShoppingList selectedList)
            {
                bool confirmed = await DisplayAlert("Confirmar", $"¿Eliminar la lista '{selectedList.Name}'?", "Sí", "No");
                if (confirmed)
                {
                    ShoppingLists.Remove(selectedList);
                    SaveShoppingLists();
                    shoppingListsListView.ItemsSource = null; // Clear binding
                    shoppingListsListView.ItemsSource = ShoppingLists; // Rebind
                }
            }
        }
        private async void ShoppingListTapped(object sender, EventArgs e)
        {
            if (shoppingListsListView.SelectedItem is ShoppingList selectedList)
            {
                await Navigation.PushAsync(new AddProductPage(selectedList, ShoppingLists));
            }
        }

        //private async void AddProductButton_Clicked(object sender, EventArgs e)
        //{
            //await Navigation.PushAsync(new AddProductPage(null, ShoppingLists));
        //}
    }
}
