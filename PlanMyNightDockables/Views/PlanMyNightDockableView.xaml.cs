using PlanMyNight.PlanMyNightDockables.ViewModels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PlanMyNight.PlanMyNightDockables.Views {
    public partial class PlanMyNightDockableView : UserControl {
        public PlanMyNightDockableView() {
            InitializeComponent();
            this.Loaded += OnLoaded;

            var vm = new PlanMyNightDockableViewModel();
            this.DataContext = vm;
            vm.ToastRequested += ShowToast;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            if (ReferenceTextBlock.Foreground is SolidColorBrush textBrush) {
                // Ajoute une ressource dynamique utilisable dans le XAML
                this.Resources["FilterWheelStrokeBrush"] = new SolidColorBrush(textBrush.Color);
            } else {
                // Fallback : si la couleur n'est pas trouvée, applique le noir
                this.Resources["FilterWheelStrokeBrush"] = new SolidColorBrush(Colors.Black);
            }
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e) { }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e) { }

        private void CheckBox_Checked_2(object sender, RoutedEventArgs e) { }

        private void CheckBox_Checked_3(object sender, RoutedEventArgs e) { }

        private void OnCalculateClicked(object sender, RoutedEventArgs e) {
            if (DataContext is PlanMyNightDockableViewModel vm) {
                vm.CalculateResults();          // ✅ Appelle la bonne méthode
                vm.UpdateFilterSegments();      // ✅ Met à jour les segments
            }
        }

        private void OnLoadProfileClicked(object sender, RoutedEventArgs e) {
            if (DataContext is PlanMyNightDockableViewModel vm) {
                vm.OnLoadProfileClicked();
            }
        }

        private void OnSaveProfileClicked(object sender, RoutedEventArgs e) {
            if (DataContext is PlanMyNightDockableViewModel vm) {
                vm.OnSaveProfileClicked();
            }
        }

        private void OnDeleteProfileClicked(object sender, RoutedEventArgs e) {
            if (DataContext is PlanMyNightDockableViewModel vm) {
                vm.OnDeleteProfileClicked();
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
