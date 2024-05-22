using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Numerics;
using System.Windows;
using System.Windows.Input;
using TaskMastery.Command;
using TaskMastery.DataAccess;
using TaskMastery.Model;

namespace TaskMastery.ViewModel
{
    internal class DashboardViewModel : ViewModelBase
    {
        readonly private UserDataTable _userDataTable;
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
        // Ajoutez une propriété pour les étiquettes
        private ObservableCollection<EtiquetteModel> _etiquettes;
        public ObservableCollection<EtiquetteModel> Etiquettes
        {
            get { return _etiquettes; }
            set
            {
                if (_etiquettes != value)
                {
                    // Vérifiez si la collection a été modifiée
                    if (_etiquettes != null)
                    {
                        _etiquettes.CollectionChanged -= EtiquettesCollectionChanged;
                    }

                    _etiquettes = value;

                    // Abonnez-vous à l'événement CollectionChanged ici
                    if (_etiquettes != null)
                    {
                        _etiquettes.CollectionChanged += EtiquettesCollectionChanged;
                    }

                    OnPropertyChanged(nameof(Etiquettes));
                }
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
        // tache sélectionnée
        private TacheModel _selectedTache;
        public TacheModel SelectedTache
        {
            get { return _selectedTache; }
            set
            {
                _selectedTache = value;
                if (_selectedTache != null)
                {
                    // Mettre à jour la table des participants
                    Participants = _userDataTable.LoadParticipantsFromDatabase(_selectedTache.Id);
                }
                OnPropertyChanged(nameof(SelectedTache));
            }
        }
        // Pseudo
        private string _pseudo;
        public string Pseudo
        {
            get { return _pseudo; }
            set
            {
                _pseudo = value;
                OnPropertyChanged(nameof(Pseudo));
            }
        }

        // Participants
        private ObservableCollection<ParticipantModel> _participants;
        public ObservableCollection<ParticipantModel> Participants
        {
            get { return _participants; }
            set
            {
                _participants = value;
                OnPropertyChanged(nameof(Participants));
            }
        }
        // Commandes
        public ICommand? ShowProjectDetailsCommand { get; }
        public ICommand? UpdateTacheCommand { get; }
        public ICommand? ShowTacheCommand { get; }
        public DashboardViewModel(string pseudo)
        {
            // Initialisez
            _participants = [];
            _projects = [];
            _selectedProject = new();
            _etiquettes = [];
            _taches = [];
            _selectedTache = new();
            _pseudo = "";
            Pseudo = pseudo;
            _userDataTable = new();
            // Initialisez les données des projets, par exemple à partir d'une base de données
            Projects = _userDataTable.LoadProjectsFromDatabase(Pseudo);

            // Afficher la liste des étiquettes
            Etiquettes = _userDataTable.LoadEtiquettesFromDatabase(Pseudo);

            // Initialisez la commande pour afficher les détails d'un projet
            ShowProjectDetailsCommand = new RelayCommand(ShowProjectDetails);

            // Initialisez la commande pour mettre à jour une tâche
            UpdateTacheCommand = new RelayCommand(UpdateTache);

            // Initialisez la commande pour afficher une tâche
            ShowTacheCommand = new RelayCommand(ShowTache);

            // Initialisez la liste des tâches
            Taches = [];
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
            return []; 
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
        private void UpdateTache(object param)
        {
            MessageBox.Show("Mise à jour de la tâche");
        }
        private void ShowTache(object param)
        {
            MessageBox.Show("Affichage de la tâche");
        }
        // Méthode pour gérer l'événement CollectionChanged
        private void EtiquettesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                // La méthode Remove a été utilisée pour supprimer un élément de la collection
                // Obtenir les éléments supprimés de la collection :
                foreach (EtiquetteModel item in e.OldItems)
                {
                    // Donner l'id de l'élément à la méthode DeleteEtiquette de la classe dataAccess
                    if (!_userDataTable.DeleteEtiquette(item.Id))
                    {
                        // Remettre l'élément dans la collection si la suppression a échoué
                        Etiquettes.Add(item);
                    }
                }
            }
            // Méthode Add a été utilisée pour ajouter un élément à la collection
            else if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (EtiquetteModel item in e.NewItems)
                {
                    // Ajouter l'élément à la base de données
                    item.Id_User = _userDataTable.GetId(Pseudo);
                    item.Id = _userDataTable.InsertEtiquette(item.Designation, item.Id_User);
                }
            }
        }

    }
}
