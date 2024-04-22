using System.Windows;
using System.Windows.Controls;

namespace TaskMastery.Assets.Components
{
    /// <summary>
    /// Logique d'interaction pour NavBarUserControl.xaml
    /// </summary>
    public partial class NavBarUserControl : UserControl
    {
        public NavBarUserControl()
        {
            InitializeComponent();
            //Souligner le contenu du btn Dashboard
            BTN_Dashboard.FontWeight = FontWeights.Bold;
        }
        private void BTN_Dashboard_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BTN_Profil_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
