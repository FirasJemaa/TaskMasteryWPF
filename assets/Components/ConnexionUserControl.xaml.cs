using Mysqlx.Prepare;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace TaskMastery.Assets.Components
{
    /// <summary>
    /// Logique d'interaction pour ConnexionUserControl.xaml
    /// </summary>
    public partial class ConnexionUserControl : UserControl
    {
        private string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d\s]).{8,}$";
        public ConnexionUserControl()
        {
            InitializeComponent();
        }        
    }
}
