using Mysqlx.Prepare;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using TaskMastery.ViewModel;

namespace TaskMastery.Assets.Components
{
    /// <summary>
    /// Logique d'interaction pour ConnexionUserControl.xaml
    /// </summary>
    public partial class ConnexionUserControl : UserControl
    {
        public ConnexionUserControl(Window parentWindow)
        {
            InitializeComponent();
            DataContext = new LogInUpViewModel(parentWindow);
        }

        private void SAI_Password_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }
    }
}
