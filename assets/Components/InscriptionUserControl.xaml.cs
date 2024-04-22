using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Transactions;

namespace TaskMastery.Assets.Components
{
    /// <summary>
    /// Logique d'interaction pour InscriptionUserControl.xaml
    /// </summary>
    public partial class InscriptionUserControl : UserControl
    {
        public InscriptionUserControl()
        {
            InitializeComponent();
        }

        private void SAI_Password_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            Force_Password();
        }
        private void Force_Password()
        {
            //faire un regex et voir la force du mot de passe en fonction on change la couleur du rectangle qui s'appelle FOR_ForcePass (tailledu mot de passe, caractères spéciaux, majuscules, minuscules, chiffres)
            int point = 0;
            
            if (SAI_Password.Password.Length >= 11)
            {
                point ++;
            }

            if (Regex.IsMatch(SAI_Password.Password, @"[\W_]"))          
            {
                point ++;
            }   

            if (Regex.IsMatch(SAI_Password.Password, @"[a-z]"))
            {
                point ++;
            }

            if (Regex.IsMatch(SAI_Password.Password, @"[A-Z]"))
            {
                point ++;
            }

            if(Regex.IsMatch(SAI_Password.Password, @"[0-9]"))
            {
                point ++;
            }

            switch (point)
                {
                case 0:
                    Transition(FOR_ForcePass.Fill, Colors.Red);
                    break;
                case 1:
                    Transition(FOR_ForcePass.Fill, Colors.Orange);
                    break;
                case 2:
                    Transition(FOR_ForcePass.Fill, Colors.DarkGoldenrod);
                    break;
                case 3:
                    Transition(FOR_ForcePass.Fill, Colors.Yellow);
                    break;
                case 4:
                    Transition(FOR_ForcePass.Fill, Colors.LightGreen);
                    break;
                case 5:
                    Transition(FOR_ForcePass.Fill, Colors.Green);
                    break;
            }

        }
        private void Transition(Brush couleurDepart, Color couleurFinal)
        {
            Color couleurDeDepart;

            if (couleurDepart is SolidColorBrush solidColorBrush)
            {
                couleurDeDepart = solidColorBrush.Color;
            }
            else
            {
                couleurDeDepart = Colors.Transparent;
            }
            ColorAnimation animation = new ColorAnimation();
            animation.From = couleurDeDepart; // Couleur de départ
            animation.To = couleurFinal; // Couleur finale
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.5)); // Durée de l'animation (0.5 seconde)
            animation.EasingFunction = new System.Windows.Media.Animation.CubicEase() { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }; // Transition ease in out

            Storyboard.SetTarget(animation, FOR_ForcePass);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Fill.Color"));

            Storyboard story = new Storyboard();
            story.Children.Add(animation);

            story.Begin();
        }
    }
}
