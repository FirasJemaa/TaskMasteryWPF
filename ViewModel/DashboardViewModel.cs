using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TaskMastery.Command;
using TaskMastery.DataAccess;
using TaskMastery.Model;

namespace TaskMastery.ViewModel
{
    internal class DashboardViewModel : ViewModelBase
    {
        private UserDataTable _userDataTable;
        private List<ProjectModel> _projects;
        public List<ProjectModel> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }
        private ProjectModel _selectedProject;
        public ProjectModel SelectedProject
        {
            get {
                UpdateTable();
                return _selectedProject; 
            }
            set
            {
                _selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
            }
        }
        private string _sCommandeTest;
        public string sCommandeTest 
        {
            get { return _sCommandeTest; }
            set
            {
                _sCommandeTest = value;
                OnPropertyChanged(nameof(sCommandeTest));
            }
        }
        // Ajoutez une propriété pour les étiquettes
        private ObservableCollection<EtiquetteModel> _etiquettes;
        public ObservableCollection<EtiquetteModel> Etiquettes
        {
            get { return _etiquettes; }
            set
            {
                MessageBox.Show("Etiquettes chargées");
                _etiquettes = value;
                OnPropertyChanged(nameof(Etiquettes));
            }
        }
        //élément étiqutte sélectionné
        private EtiquetteModel _selectedEtiquette;
        public EtiquetteModel SelectedEtiquette
        {
            get { return _selectedEtiquette; }
            set
            {
                _selectedEtiquette = value;
                OnPropertyChanged(nameof(SelectedEtiquette));
            }
        }

        // Liste de tache qui sera affichée dans le tableau
        private ObservableCollection<TacheModel> _taches;
        public ObservableCollection<TacheModel> Taches
        {
            get { return _taches; }
            set
            {
                _taches = value;
                OnPropertyChanged(nameof(Taches));
            }
        }
        public ICommand ShowProjectDetailsCommand { get; }
        public ICommand UpdateTacheCommand { get; }
        public DashboardViewModel(string pseudo)
        {
            _userDataTable = new UserDataTable();
            // Initialisez les données des projets, par exemple à partir d'une base de données
            Projects = _userDataTable.LoadProjectsFromDatabase(pseudo);

            // Afficher la liste des étiquettes
            Etiquettes = _userDataTable.LoadEtiquettesFromDatabase(pseudo);

            // Initialisez la commande pour afficher les détails d'un projet
            ShowProjectDetailsCommand = new RelayCommand(ShowProjectDetails);

            // Initialisez la commande pour mettre à jour une tâche
            UpdateTacheCommand = new RelayCommand(UpdateTache);
        }

        private void ShowProjectDetails(object parameter)
        {
            // Implémentez la logique pour afficher les détails du projet sélectionné
            if (_selectedProject != null)
            {
                //
            }
        }

        // Méthode pour charger les projets depuis la base de données
        private List<ProjectModel> LoadProjectsFromDatabase(string pseudo)
        {
            // Retournez la liste des projets chargés
            return new List<ProjectModel>(); 
        }

        public void UpdateTable()
        {
            //changer la propriété
            if (_selectedProject != null)
            {
                //Recuperer la liste de tache et le retourner dans la propriété
                Taches = _userDataTable.LoadTachesFromDatabase(_selectedProject.Id);
            }
        }

        private void AddNewItem()
        {
            // Logique à exécuter lors de l'ajout d'un nouvel élément
            MessageBox.Show("Ajout d'un nouvel élément");
        }

        private void CellEditEnding()
        {
            // Logique à exécuter lors de la fin de l'édition d'une cellule
            MessageBox.Show("Fin de l'édition d'une cellule");
        }

        private void UpdateTache(object param)
        {
            MessageBox.Show("Mise à jour de la tâche");
        }
    }
}
