using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppBanka.Model;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace AppBanka.MenuPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListDetailsPage : ContentPage
    {
        private List<ShoppingList> ShoppingLists { get; set; }
        public ShoppingList SelectedList { get; set; }
        private bool IsEditMode { get; set; } = false;

        public ListDetailsPage(ShoppingList selectedList, List<ShoppingList> shoppingLists)
        {
            InitializeComponent();
            SelectedList = selectedList;
            ShoppingLists = shoppingLists;
            BindingContext = this;
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
                SelectedList.Name = listName;
                SaveShoppingLists();
                ToggleEditMode();
            }
        }

        private void ToggleEditMode()
        {
            IsEditMode = !IsEditMode;
            OnPropertyChanged(nameof(IsEditMode));
        }
    }
}
