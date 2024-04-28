using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using TaskMastery.Model;
using TaskMastery.ViewModel;

namespace TaskMastery.Assets.Components
{
    /// <summary>
    /// Logique d'interaction pour DashboardUserControl.xaml
    /// </summary>
    public partial class DashboardUserControl : UserControl
    {
        public DashboardUserControl(string pseudo)
        {
            InitializeComponent();
            DashboardViewModel dashboardViewModel = new DashboardViewModel(pseudo);
            this.DataContext = dashboardViewModel;            
        }

        private void Tbl_Etiquette_Loaded(object sender, RoutedEventArgs e)
        {
            if (Tbl_Etiquette.Columns.Count > 0)
            { 
                Tbl_Etiquette.Columns[0].Visibility = Visibility.Collapsed;
                Tbl_Etiquette.Columns[2].Visibility = Visibility.Collapsed;
            }
        }

        private void Tbl_Etiquette_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            // Ajout d'une nouvelle étiquette
            EtiquetteModel etiquette = new EtiquetteModel();
            etiquette.Id = 0;
            etiquette.Designation = "";
        }

        private void Tbl_Tache_Loaded(object sender, RoutedEventArgs e)
        {
            if (Tbl_Tache.Columns.Count > 0)
            {
                Tbl_Tache.Columns[0].Visibility = Visibility.Collapsed;
            }
        }

        private void Tbl_Tache_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //afficher le statut de la tâche qui a été double-cliquée
            MessageBox.Show(Tbl_Tache.SelectedItem.ToString());
        }
    }
}
