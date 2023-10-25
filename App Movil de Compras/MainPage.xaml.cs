using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using AppBanka.Model;
using System.Linq;
using Xamarin.Essentials;

namespace AppBanka

{
    public partial class MainPage : ContentPage
    {
        private int incorrectAttempts = 0;
        private bool isAppClosing = false;

        private string url = "https://gcb900465b2cf2d-apfnhyqu544f6j59.adb.us-phoenix-1.oraclecloudapps.com/ords/admin/dusers/users";
        HttpClient client = new HttpClient();
        private int lastGeneratedId;

        public IList<CAT_USUARIO> CAT_USUARIOSS { get; private set; }

        public MainPage()
        {
            InitializeComponent();

            CAT_USUARIOSS = new List<CAT_USUARIO>();
            CAT_USUARIOSS.Add(new CAT_USUARIO
            {
                id = "1",
                nombre = "Fer",
                clave = "999",
                activa = "0"
            });

            BindingContext = this;
            lblstatus.Text = "( Mensajes )";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Preferences.ContainsKey("LastGeneratedId"))
            {
                lastGeneratedId = Preferences.Get("LastGeneratedId", 10);
            }
            else
            {
                lastGeneratedId = 10;
            }
        }

        private async Task<bool> BuscarUsuarioAsync(string nombre, string clave)
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string contenido = await response.Content.ReadAsStringAsync();
                ArrayProblem arrayProblem = JsonConvert.DeserializeObject<ArrayProblem>(contenido);

                bool usuarioExiste = arrayProblem.items.Any(u => u.nombre == nombre && u.clave == clave);
                return usuarioExiste;
            }
            else
            {
                return false;
            }
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            string nombre = txtnombre.Text;
            string clave = txtclave.Text;

            bool usuarioExiste = await BuscarUsuarioAsync(nombre, clave);

            if (usuarioExiste)
            {
                incorrectAttempts = 0;
                await Navigation.PushAsync(new HomePage()); // Navigate to the HomePage
            }
            else
            {
                incorrectAttempts++;

                if (incorrectAttempts == 3)
                {
                    await DisplayAlert("Cuidado", "La aplicación se cerrará con un intento fallido más.", "OK");
                }
                else if (incorrectAttempts > 3)
                {
                    await DisplayAlert("Lo Sentimos", "Por motivos de seguridad, la aplicación será cerrada.", "OK");
                    CloseApp();
                }
                else
                {
                    lblstatus.Text = "Usuario no existe, intente nuevamente";
                }
            }
        }

        private async Task<bool> RegisterUserAsync(string nombre, string clave, string activa)
        {
            try
            {
                string id = (lastGeneratedId++).ToString();

                var requestData = new
                {
                    id = id,
                    nombre = nombre,
                    clave = clave,
                    activa = activa
                };

                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    lblstatus.Text = "Error: " + errorMessage;
                    return false;
                }
            }
            catch (Exception ex)
            {
                lblstatus.Text = "Error: " + ex.Message;
                return false;
            }
        }

        private async void BtnSignup_Clicked(object sender, EventArgs e)
        {
            string nombre = txtnombre.Text;
            string clave = txtclave.Text;

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(clave))
            {
                await DisplayAlert("Registro Fallido", "Por favor complete todos los campos.", "OK");
                return;
            }

            bool acceptedPolicy = await DisplayAlert("Aceptar Políticas", "¿Aceptas nuestras políticas de uso?", "Aceptar", "Rechazar");

            if (acceptedPolicy)
            {
                string activa = "1";

                bool registrationSuccess = await RegisterUserAsync(nombre, clave, activa);

                if (registrationSuccess)
                {
                    await DisplayAlert("Bienvenido " + nombre, "Esperamos que disfrutes de nuestro sistema de lista de compras", "OK");

                    lblstatus.Text = "Usuario registrado exitosamente";
                    Preferences.Set("LastGeneratedId", lastGeneratedId);
                }
                else
                {
                    lblstatus.Text = "Error al registrar el usuario";
                }
            }
            else
            {
                await DisplayAlert("Políticas Rechazadas", "Lamentamos que no aceptes nuestras políticas, por ese motivo no te puedes registrar en nuestra aplicación. Te recomendamos leer nuevamente las políticas e intentar nuevamente para no perderte de nuestra increíble aplicación.", "OK");
            }
        }

        private async void BtnLimpiar_Clicked(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtnombre.Text = string.Empty;
            txtclave.Text = string.Empty;
            lblstatus.Text = "( Mensajes )";
        }

   


        private async void CloseApp()
        {
            if (isAppClosing)
            {
                await Task.Delay(2000);
                Device.BeginInvokeOnMainThread(() =>
                {
                    Xamarin.Essentials.AppInfo.ShowSettingsUI();
                });
            }
        }
    }
}
