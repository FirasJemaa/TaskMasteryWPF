using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
            }
        }
        public ICommand ShowProjectDetailsCommand { get; }

        public DashboardViewModel(string pseudo)
        {
            _userDataTable = new UserDataTable();
            // Initialisez les données des projets, par exemple à partir d'une base de données
            Projects = _userDataTable.LoadProjectsFromDatabase(pseudo);

            // Initialisez la commande pour afficher les détails d'un projet
            ShowProjectDetailsCommand = new RelayCommand(ShowProjectDetails);
        }

        private void ShowProjectDetails(object parameter)
        {
            // Implémentez la logique pour afficher les détails du projet sélectionné
            // Vous pouvez utiliser la propriété SelectedProject pour accéder au projet sélectionné
            // par exemple : MessageBox.Show("Détails du projet : " + SelectedProject.Name);
        }

        // Méthode pour charger les projets depuis la base de données
        private List<ProjectModel> LoadProjectsFromDatabase(string pseudo)
        {
            // Implémentez la logique pour charger les projets à partir de la base de données
            // Utilisez le pseudo pour filtrer les projets selon l'utilisateur connecté, si nécessaire
            // Retournez la liste des projets chargés
            return new List<ProjectModel>(); // Placeholder, remplacez-le par la vraie implémentation
        }
    }
}
