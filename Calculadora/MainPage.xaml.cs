using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BASE
{
    public partial class MainPage : ContentPage
    {
        private int operacionCount = 1;
        private string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "operaciones.txt");
        //Esta linea me crea el archivo de texto para guardar las operaciones
        public MainPage()
        {
            //operadores
            InitializeComponent();
            CL.Clicked += CL_Clicked; 
            suma.Clicked += suma_Clicked; 
            resta.Clicked += resta_Clicked; 
            multi.Clicked += multi_Clicked;
            div.Clicked += div_Clicked;

            // Agregar los botones al Toolbar
            var guardarButton = new ToolbarItem
            {
                Text = "Guardar",
                Priority = 0,
                Order = ToolbarItemOrder.Primary,
            };
            guardarButton.Clicked += GuardarButton_Clicked;
            ToolbarItems.Add(guardarButton);

            var verButton = new ToolbarItem
            {
                IconImageSource = "ver_icon.png",
                Priority = 1,
                Order = ToolbarItemOrder.Primary,
            };
            verButton.Clicked += VerButton_Clicked;
            ToolbarItems.Add(verButton);
        }

        //Seccion de metodos de operaciones
        private void suma_Clicked(object sender, EventArgs e)
        {
            if (int.TryParse(Entrada1.Text, out int n1) && int.TryParse(Entrada2.Text, out int n2))
            {
                int res = n1 + n2;
                Resultado.Text = res.ToString();
                GuardarOperacion($"{n1} + {n2} = {res}");
            }
            else
            {
                MostrarMensajeAdvertencia("Ambos valores deben ser números enteros.");
            }

        }
        private void resta_Clicked(object sender, EventArgs e)
        {
            if (int.TryParse(Entrada1.Text, out int n1) && int.TryParse(Entrada2.Text, out int n2))
            {
                int res = n1 - n2;
                Resultado.Text = res.ToString();
                GuardarOperacion($"{n1} - {n2} = {res}");
            }
            else
            {
                MostrarMensajeAdvertencia("Ambos valores deben ser números enteros.");
            }
        }
        private void multi_Clicked(object sender, EventArgs e)
        {
            if (int.TryParse(Entrada1.Text, out int n1) && int.TryParse(Entrada2.Text, out int n2))
            {
                int res = n1 * n2;
                Resultado.Text = res.ToString();
                GuardarOperacion($"{n1} * {n2} = {res}");
            }
            else
            {
                MostrarMensajeAdvertencia("Ambos valores deben ser números enteros.");
            }
        }

        private void div_Clicked(object sender, EventArgs e)
        {
            if (int.TryParse(Entrada1.Text, out int n1) && int.TryParse(Entrada2.Text, out int n2))
            {
                if (n2 != 0)
                {
                    int res = n1 / n2;
                    Resultado.Text = res.ToString();
                    GuardarOperacion($"{n1} / {n2} = {res}");
                }
                else
                {
                    MostrarMensajeAdvertencia("No se puede dividir por cero.");
                }
            }
            else
            {
                MostrarMensajeAdvertencia("Ambos valores deben ser números enteros.");
            }
        }
        //Fin de la seccion de metodos de operaciones
        
        //Metodo para limpiar el resultado
        private void CL_Clicked(object sender, EventArgs e)
        {
            Entrada1.Text = "";
            Entrada2.Text = "";
            Resultado.Text = "0.00";
        }

        //En esta parte esta cubierto lo de que no se guarda automaticamente si no que hay que darle guardar
        private async void GuardarButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Resultado.Text))
            {
                GuardarOperacion($"{Resultado.Text}");
                await DisplayAlert("Operación Guardada", "La operación ha sido guardada.", "Aceptar");
            }
            else
            {
                await DisplayAlert("Advertencia", "No hay resultado para guardar.", "Aceptar");
            }
        }

        private async void VerButton_Clicked(object sender, EventArgs e)
        {
                verButton.IsEnabled = false; // Deshabilita el boton "Ver" temporalmente hasta que haya alguna operacion

            // Leer el contenido del archivo de texto
            string contenido;
            using (StreamReader reader = new StreamReader(filePath))
            {
                contenido = await reader.ReadToEndAsync();
            }

            //Esto muestra las operaciones en cuadro de dialogo
            await DisplayAlert("Operaciones Guardadas", contenido, "Aceptar");

            verButton.IsEnabled = true; // Volver a habilitar el boton "Ver"
        }

        private void GuardarOperacion(string operacion)//bueno esto solo guarda las operaciones en el txt
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{operacionCount}. {operacion}");
                operacionCount++;//aca ya esta cubierto lo del acumulador de operaciones
                //el cuadro de dialogo muestra todas las operaciones realizadas
           }
        }

        private void MostrarMensajeAdvertencia(string mensaje)
        {
            DisplayAlert("Advertencia", mensaje, "Aceptar");
        }
    }
}
