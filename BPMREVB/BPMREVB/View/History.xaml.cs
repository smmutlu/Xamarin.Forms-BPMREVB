using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BPMREVB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class History : ContentPage
    {
        public History()
        {
            InitializeComponent();
        }

        async void OnButton1Clicked(object sender, EventArgs args)
        {
            Image image = (Image) sender;
            image.Opacity = 0.75;
            //var value = image.
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                image.Opacity = 1;
                return false;
            });
            
        }

        async void OnButton2Clicked(object sender, EventArgs args)
        {
            Image image = (Image) sender;
            image.Opacity = 0.75;

            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                image.Opacity = 1;
                return false;
            });
            await Navigation.PushAsync(new Stats());

        }

        async void OnButton3Clicked(object sender, EventArgs args)
        {
            Image image = (Image) sender;
            image.Opacity = 0.75;
            //var value = image.
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                image.Opacity = 1;
                return false;
            });
            await Navigation.PushAsync(new Settings());

        }
    }
}
