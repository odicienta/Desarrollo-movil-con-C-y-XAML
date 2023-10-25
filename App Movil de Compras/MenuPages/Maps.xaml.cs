using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace AppBanka
{
    public partial class Maps : ContentPage
    {
        public Maps()
        {
            InitializeComponent();
            BtnGps.Clicked += BtnGps_Clicked;
            btnMap.Clicked += BtnMap_Clicked;
            btnMail.Clicked += BtnMail_Clicked;

        }

        private void BtnMail_Clicked(object sender, EventArgs e)
        {

            // String Body = "Este es el contenido del mensaje";
            String Body = "<h1 style=\"color: #333; margin-top: 40px;\">Genial!</h1><span style=\"font-style: italic; color: #666;\">Disfruta de Tu tienda en : 9.941212 -84.0780812. Saludos por parte de Basket</span>";

            List<string> Mailers = new List<string>();
            Mailers.Add(EntryEmail.Text);
            //Mailers.Add("elcorreo@server.com");
            SendEmail(EntrySubject.Text, Body, Mailers);


        }

        public async Task SendEmail(string subject, string body, List<string> recipientes)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipientes,
                    //Cc = ccRecipientes,
                    //Bcc = bccRecipientes
                    BodyFormat = EmailBodyFormat.Html
                    //BodyFormat = EmailBodyFormat.PlainText
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email no soportado en este dispositivo
            }
            catch (Exception ex)
            {
                // Error diferente
            }
        }


        private void BtnMap_Clicked(object sender, EventArgs e)
        {


            if (!double.TryParse(EntryLatitude.Text, out double lat)) { return; }
            if (!double.TryParse(EntryLongitud.Text, out double lng)) { return; }
            Map.OpenAsync(lat, lng, new MapLaunchOptions
            {
                
                NavigationMode = NavigationMode.None
            });



        }

        private void BtnGps_Clicked(object sender, EventArgs e)
        {
            ObtenerUbica();
        }


        private async void ObtenerUbica()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }
                if (location == null)
                {
                    lblLocaliza.Text = "No hay GPS";
                }
                else
                {
                    lblLocaliza.Text = $"{location.Latitude}{location.Longitude}";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Algo sale mal :{ex.Message}");
            }
        }


        private void OnLocationPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = LocationPicker.SelectedIndex;

            if (selectedIndex == 0) // Lugar 1
            {
                EntryLatitude.Text = "9.941212";
                EntryLongitud.Text = "-84.0780812";
            }
            else if (selectedIndex == 1) // Lugar 2
            {
                EntryLatitude.Text = "9.912345";
                EntryLongitud.Text = "-83.1234567";
            }
        }
    }
}