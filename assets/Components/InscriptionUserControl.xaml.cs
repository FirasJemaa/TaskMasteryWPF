using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Transactions;
using TaskMastery.ViewModel;

namespace TaskMastery.Assets.Components
{
    /// <summary>
    /// Logique d'interaction pour InscriptionUserControl.xaml
    /// </summary>
    public partial class InscriptionUserControl : UserControl
    {
        public InscriptionUserControl(Window parentWindow)
        {
            InitializeComponent();
            DataContext = new LogInUpViewModel(parentWindow);
        }        
    }
}
