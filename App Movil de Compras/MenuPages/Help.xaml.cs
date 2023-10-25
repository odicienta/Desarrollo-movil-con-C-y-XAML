using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AppBanka
{	
	public partial class Help : ContentPage
	{
		public Help()
		{
			InitializeComponent();
			



		}

        private void OnEconomyTipsButtonClicked(object sender, EventArgs e)
        {
            // URL for household economy tips
            string tipsUrl = "https://www.cresory.com/blog/economia-domestica";

            Device.OpenUri(new Uri(tipsUrl));
        }


    }
}

