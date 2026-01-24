using Microsoft.Maui.Controls;

namespace HealPages
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent(); // Must be present
            MainPage = new Views.MainPage();
        }
    }
}
