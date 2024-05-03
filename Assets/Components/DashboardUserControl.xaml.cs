using System.Windows.Controls;
using TaskMastery.Model;
using TaskMastery.ViewModel;

namespace TaskMastery.Assets.Components
{
    public partial class DashboardUserControl : UserControl
    {
        private DashboardViewModel dashboardViewModel;
        private string pseudo;
        public DashboardUserControl(string _pseudo)
        {
            pseudo = _pseudo;
            InitializeComponent();
            dashboardViewModel = new DashboardViewModel(pseudo);
            this.DataContext = dashboardViewModel;            
        }

        private void Tbl_Etiquette_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            // Ajout d'une nouvelle étiquette
            EtiquetteModel etiquette = new EtiquetteModel(pseudo);
            etiquette.Id = 0;
            etiquette.Designation = "";
            etiquette.Id_User = etiquette.id_userCurrent;
        }
    }
}
