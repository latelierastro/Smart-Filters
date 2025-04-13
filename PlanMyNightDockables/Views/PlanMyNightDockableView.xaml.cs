using System.Threading.Tasks;
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
        private void OnLoadProfileClicked(object sender, RoutedEventArgs e) {
            if (DataContext is PlanMyNightDockables.ViewModels.PlanMyNightDockableViewModel vm) {
                vm.OnLoadProfileClicked();
                ShowToast($"📂 Profile '{vm.SelectedProfileName}' loaded.");
            }
        }


        private void OnSaveProfileClicked(object sender, RoutedEventArgs e) {
            if (DataContext is PlanMyNightDockables.ViewModels.PlanMyNightDockableViewModel vm) {
                vm.OnSaveProfileClicked();
                ShowToast($"✅ Profile '{vm.SelectedProfileName}' saved.");
            }
        }


        private async void ShowToast(string message) {
            ToastText.Text = message;
            ToastNotification.Visibility = Visibility.Visible;

            await Task.Delay(2000); // attends 2 secondes

            ToastNotification.Visibility = Visibility.Collapsed;
        }

    }
}
