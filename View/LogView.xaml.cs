using System.Windows;
using TaskMastery.Assets.Components;

namespace TaskMastery.View
{
    public partial class LogView : Window
    {
        readonly InscriptionUserControl _inscriptionUserControl;
        readonly ConnexionUserControl _connexionUserControl;
        public LogView()
        {
            InitializeComponent();
            _inscriptionUserControl = new InscriptionUserControl(this);
            _connexionUserControl = new ConnexionUserControl(this);
            Btn_Changed_Window.Content = "Inscription";
            this.Title = "Connexion";

            GR_Champs.Children.Add(_connexionUserControl);
        }
        private void Btn_Changed_Window_Click(object sender, RoutedEventArgs e)
        {
            GR_Champs.Children.Clear();
            if (Btn_Changed_Window.Content.ToString() == "Connexion")
            {
                Btn_Changed_Window.Content = "Inscription";
                this.Title = "Connexion";
                GR_Champs.Children.Add(_connexionUserControl);
            }
            else
            {
                Btn_Changed_Window.Content = "Connexion";
                this.Title = "Inscription";
                GR_Champs.Children.Add(_inscriptionUserControl);
            }
        }
    }
}
