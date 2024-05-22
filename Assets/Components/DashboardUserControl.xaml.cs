using System.Windows.Controls;
using TaskMastery.Model;
using TaskMastery.ViewModel;

namespace TaskMastery.Assets.Components
{
    public partial class DashboardUserControl : UserControl
    {
        private readonly DashboardViewModel dashboardViewModel;
        private readonly string pseudo;
        public DashboardUserControl(string _pseudo)
        {
            pseudo = _pseudo;
            InitializeComponent();
            dashboardViewModel = new DashboardViewModel(pseudo);
            this.DataContext = dashboardViewModel;            
        }
    }
}
