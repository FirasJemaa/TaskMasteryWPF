using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace TaskMastery.Assets.Components
{
    /// <summary>
    /// Logique d'interaction pour PasswordUserControl.xaml
    /// </summary>
    public partial class PasswordUserControl : UserControl
    {
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordUserControl), new PropertyMetadata(""));

        public PasswordUserControl()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = PasswordBox.Password;
            Force_Password();
        }
        private void Force_Password()
        {
            //faire un regex et voir la force du mot de passe en fonction on change la couleur du rectangle qui s'appelle FOR_ForcePass (tailledu mot de passe, caractères spéciaux, majuscules, minuscules, chiffres)
            int point = 0;

            if (PasswordBox.Password.Length >= 11)
            {
                point++;
            }

            if (Regex.IsMatch(PasswordBox.Password, @"[\W_]"))
            {
                point++;
            }

            if (Regex.IsMatch(PasswordBox.Password, @"[a-z]"))
            {
                point++;
            }

            if (Regex.IsMatch(PasswordBox.Password, @"[A-Z]"))
            {
                point++;
            }

            if (Regex.IsMatch(PasswordBox.Password, @"[0-11]"))
            {
                point++;
            }

            switch (point)
            {
                case 0:
                    Transition(PasswordBox.BorderBrush, Colors.Red);
                    break;
                case 1:
                    Transition(PasswordBox.BorderBrush, Colors.Orange);
                    break;
                case 2:
                    Transition(PasswordBox.BorderBrush, Colors.DarkGoldenrod);
                    break;
                case 3:
                    Transition(PasswordBox.BorderBrush, Colors.Yellow);
                    break;
                case 4:
                    Transition(PasswordBox.BorderBrush, Colors.LightGreen);
                    break;
                case 5:
                    Transition(PasswordBox.BorderBrush, Colors.Green);
                    break;
            }

        }
        private void Transition(Brush couleurDepart, Color couleurFinal)
        {
            if (couleurDepart is SolidColorBrush couleurDeDepart)
            {
                // Créer une nouvelle instance de SolidColorBrush avec la couleur finale
                SolidColorBrush nouvelleCouleur = new(couleurFinal);

                // Créer une animation de transition de la couleur de la bordure
                ColorAnimation animation = new()
                {
                    From = couleurDeDepart.Color, // Couleur de départ
                    To = couleurFinal, // Couleur finale
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)), // Durée de l'animation (0.5 seconde)
                    EasingFunction = new System.Windows.Media.Animation.CubicEase() { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut } // Transition ease in out
                };

                // Définir la cible de l'animation sur la nouvelle couleur
                Storyboard.SetTarget(animation, nouvelleCouleur);
                Storyboard.SetTargetProperty(animation, new PropertyPath(SolidColorBrush.ColorProperty)); // Animation de la propriété Color

                // Créer et démarrer le storyboard de l'animation
                Storyboard story = new();
                story.Children.Add(animation);
                story.Begin();

                // Appliquer la nouvelle couleur à la propriété BorderBrush du PasswordBox
                PasswordBox.BorderBrush = nouvelleCouleur;
            }
        }
    }
}