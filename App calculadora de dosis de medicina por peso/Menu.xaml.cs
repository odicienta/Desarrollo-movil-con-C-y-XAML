using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App9
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Menu : ContentPage
	{
		public Menu ()
		{
			InitializeComponent ();
            RegistroPeso.Clicked += RegistroPeso_Clicked;
        }
        private async void RegistroPeso_Clicked(object sender, EventArgs e)
        {
            ((NavigationPage)this.Parent).PushAsync(new RegistroPeso());
        }

        private void ListaMedicamentos_Clicked(object sender, EventArgs e)
        {
            // Acciones cuando se hace clic en "Lista de Medicamentos"
        }

        private void HorasSueno_Clicked(object sender, EventArgs e)
        {
            // Acciones cuando se hace clic en "Horas de Sueño"
        }

    }
}