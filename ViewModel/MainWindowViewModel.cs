using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskMastery.Assets.Components;
using TaskMastery.Command;
using TaskMastery.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    private UserControl _currentUserControl;
    public UserControl CurrentUserControl
    {
        get { return _currentUserControl; }
        set
        {
            _currentUserControl = value;
            OnPropertyChanged(nameof(CurrentUserControl));
        }
    }

    public string Pseudo { get; set; }
    public Window CurrentWindow { get; set; }

    // Commande pour afficher le UserControl ProfileUserControl
    public ICommand ShowProfileCommand { get; }
    public ICommand ShowDashboardCommand { get; }
    public MainWindowViewModel(string _pseudo, Window currentWindow)
    {
        // Initialisez la commande pour afficher le UserControl ProfileUserControl
        ShowProfileCommand = new RelayCommand(ShowProfile);
        ShowDashboardCommand = new RelayCommand(ShowDashboard);
        // Par défaut, affichez un autre UserControl ici si nécessaire
        CurrentUserControl = new DashboardUserControl(_pseudo);
        // Affectez la valeur de _pseudo à la propriété Pseudo
        Pseudo = _pseudo;
        // Affectez la valeur de currentWindow à la propriété CurrentWindow
        CurrentWindow = currentWindow;
    }

    private void ShowProfile(object parameter)
    {
        // Créez une instance du UserControl ProfileUserControl et affectez-la à CurrentUserControl
        CurrentUserControl = new ProfileUserControl(Pseudo, CurrentWindow);
    }
    private void ShowDashboard(object parameter)
    {
        // Créez une instance du UserControl ProfileUserControl et affectez-la à CurrentUserControl
        CurrentUserControl = new DashboardUserControl(Pseudo);
    }
}
