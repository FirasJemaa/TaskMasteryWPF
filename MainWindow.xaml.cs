using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskMastery.Assets.Components;

namespace TaskMastery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string _user)
        {
            InitializeComponent();
            this.Title = "Bienvenue " + _user;
            //utiliser le user control NavBarUserControl dans le grid navbar
            NavBarUserControl navBarUserControl = new NavBarUserControl();
            gridNavBar.Children.Add(navBarUserControl);
        }
    }
}