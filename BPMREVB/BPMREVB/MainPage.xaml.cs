using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BPMREVB
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}
	    void OnImageTapped(object sender, EventArgs args)
	    {
	        Image image = (Image)sender;
	        image.Opacity = 0.75;
	        //var value = image.
	        Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
	        {
	            image.Opacity = 1;
	            return false;
	        });

	        //label.Text = String.Format("Rendered Image is {0} x {1}",
	        //    image.Width, image.Height);


	    }

	    private void OnButton1Clicked(object sender, EventArgs args)
	    {
	        Image image = (Image)sender;
	        image.Opacity = 0.75;
	        //var value = image.
	        Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
	        {
	            image.Opacity = 1;
	            return false;
	        });
	//        await Navigation.PushAsync(new History());
        }

	    private void OnButton2Clicked(object sender, EventArgs args)
	    {
	        Image image = (Image)sender;
	        image.Opacity = 0.75;
	        //var value = image.
	        Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
	        {
	            image.Opacity = 1;
	            return false;
	        });

        }
	    private void OnButton3Clicked(object sender, EventArgs args)
	    {
	        Image image = (Image)sender;
	        image.Opacity = 0.75;
	        //var value = image.
	        Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
	        {
	            image.Opacity = 1;
	            return false;
	        });

	    }
    }
}
