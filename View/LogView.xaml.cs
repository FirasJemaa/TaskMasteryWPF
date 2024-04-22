using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskMastery.Assets.Components;

namespace TaskMastery.View
{
    public partial class LogView : Window
    {
        InscriptionUserControl _inscriptionUserControl = new InscriptionUserControl();
        ConnexionUserControl _connexionUserControl = new ConnexionUserControl();
        public LogView()
        {
            InitializeComponent();
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
