using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

//Done by Gabriel Nuñez Pizarro 

namespace App9
{
    public partial class MainPage : ContentPage
    {
        // Aqui se guarda las contras y users 
        private Dictionary<string, string> users; 

        public MainPage()
        {
            InitializeComponent();
            BtnIr.Clicked += BtnIr_Clicked;
            BtnRegistrarse.Clicked += BtnRegistrarse_Clicked;

            users = new Dictionary<string, string>();
        }

        private async void BtnIr_Clicked(object sender, EventArgs e)
        {
            string username = usr.Text;
            string password = pwd.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Debe ingresar un nombre de usuario y contraseña", "Aceptar");
            }
            else if (users.ContainsKey(username) && users[username] == Encrypt(password))
            {
                //await DisplayAlert("Éxito", "Inicio de sesión exitoso", "Aceptar");
                // aqui es donde se mete la otra ventana si el login es zucsefuuuuul UwU
                ((NavigationPage)this.Parent).PushAsync(new Menu());
            }
            else
            {
                await DisplayAlert("Error", "Nombre de usuario o contraseña incorrectos", "Aceptar");
            }
        }

        private async void BtnRegistrarse_Clicked(object sender, EventArgs e)
        {
            string username = usr.Text;
            string password = pwd.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Debe ingresar un nombre de usuario y contraseña", "Aceptar");
            }
            else if (users.ContainsKey(username))
            {
                await DisplayAlert("Error", "El nombre de usuario ya está registrado", "Aceptar");
            }
            else
            {
                // Guardado de credenciales seguras con encrptacion 
                users.Add(username, Encrypt(password));
                await DisplayAlert("Éxito", "Registro exitoso", "Aceptar");
                // aqui tambien se tiene que meter la ventana que sigue pero despues del register zucsesfuuuuul Uwu Omaiwa.
            }
        }

        private string Encrypt(string password)
        {
            return password; 
        }


    }
}
