using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EdubotRemote.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectView : ContentPage
    {
        public ConnectView()
        {
            InitializeComponent();

            CircleScanner.Colors.Add((Color)Application.Current.Resources["LightTeal"]);
            CircleScanner.Colors.Add((Color)Application.Current.Resources["Teal"]);
        }
    }
}