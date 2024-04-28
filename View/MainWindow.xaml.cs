using System.Windows;

namespace TaskMastery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string _pseudo)
        {
            InitializeComponent();
            this.Title = "Bienvenue " + _pseudo;
            //Initialisez le DataContext de la fenêtre principale avec MainWindowViewModel
            Window _window = this;
            DataContext = new MainWindowViewModel(_pseudo, _window);
            //Mettre le btn dashboard en gras
            BTN_Dashboard.FontWeight = FontWeights.Bold;
        }

        private void BTN_Dashboard_Click(object sender, RoutedEventArgs e)
        {
            //Mettre le btn dashboard en gras
            BTN_Dashboard.FontWeight = FontWeights.Bold;
            BTN_Profil.FontWeight = FontWeights.Normal;
        }

        private void BTN_Profil_Click(object sender, RoutedEventArgs e)
        {
            //Mettre le btn profil en gras
            BTN_Profil.FontWeight = FontWeights.Bold;
            BTN_Dashboard.FontWeight = FontWeights.Normal;
        }
    }
}