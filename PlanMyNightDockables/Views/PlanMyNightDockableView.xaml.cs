using System.Windows;
using System.Windows.Controls;

namespace PlanMyNight.PlanMyNightDockables.Views {
    public partial class PlanMyNightDockableView : UserControl {
        public PlanMyNightDockableView() {
            InitializeComponent();

            // Set the DataContext to your ViewModel
            this.DataContext = new PlanMyNightDockables.ViewModels.PlanMyNightDockableViewModel();
        }

        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e) {

        }

        private void CheckBox_Checked_1(object sender, System.Windows.RoutedEventArgs e) {

        }

        private void CheckBox_Checked_2(object sender, System.Windows.RoutedEventArgs e) {

        }

        private void CheckBox_Checked_3(object sender, System.Windows.RoutedEventArgs e) {

        }

        private void OnCalculateClicked(object sender, RoutedEventArgs e) {
            if (DataContext is PlanMyNightDockables.ViewModels.PlanMyNightDockableViewModel vm) {
                vm.CalculateResults();
            }
        }

    }
}
