using AppBanka.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBanka
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        public Menu()
        {
            InitializeComponent();

        }
    


        private async void Help_Clicked(object sender, EventArgs e)
        {
         
           
            await Navigation.PushAsync(new Help());



        }

        private async void CreateListButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateListPage());
        }

        private async void ViewListsButton_Clicked(object sender, EventArgs e)
        {
            // Handle button click logic here

            await Navigation.PushAsync(new CreateListPage());

        }

        private async void Mapa_Clicked(object sender, EventArgs e)
        {


            await Navigation.PushAsync(new Maps());



        }
        private void ResetPreferencesButton_Clicked(object sender, EventArgs e)
        {
            Preferences.Clear(); // Reiniciar las preferencias
            // Aquí puedes realizar cualquier otra acción necesaria después de reiniciar las preferencias
        }




    }

}