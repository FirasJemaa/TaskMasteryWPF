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
        }
    }
}