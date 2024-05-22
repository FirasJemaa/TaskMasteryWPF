using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using TaskMastery.ViewModel;

namespace TaskMastery.Assets.Components
{
    /// <summary>
    /// Logique d'interaction pour ProfileUserControl.xaml
    /// </summary>
    public partial class ProfileUserControl : UserControl
    {
        public ProfileUserControl(string pseudo, Window CurrentWindow)
        {
            InitializeComponent();
            //utiliser UserViewModel pour afficher les informations de l'utilisateur grace à son pseudo
            LogInUpViewModel _User = new(pseudo, CurrentWindow);
            DataContext = _User;
        }
    }
}
