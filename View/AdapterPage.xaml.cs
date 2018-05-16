using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalBPM.View
{
	public partial class AdapterPage : TabbedPage
	{
		public AdapterPage ()
		{
		    
            InitializeComponent ();
		    NavigationPage.SetHasNavigationBar(this, false);
        }
	}
}