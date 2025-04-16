using PlanMyNight.PlanMyNightDockables.ViewModels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PlanMyNight.PlanMyNightDockables.Views {

    /// <summary>
    /// CLASSE PRINCIPALE DE LA VUE UTILISATEUR POUR LE DOCKABLE PLANMYNIGHT
    /// </summary>
    public partial class PlanMyNightDockableView : UserControl {

        /// <summary>
        /// CONSTRUCTEUR : INITIALISE LA VUE, LE DATACONTEXT ET LES ÉVÉNEMENTS
        /// </summary>
        public PlanMyNightDockableView() {
            InitializeComponent();
            this.Loaded += OnLoaded;

            var vm = new PlanMyNightDockableViewModel();
            this.DataContext = vm;

            // ÉVÉNEMENT POUR AFFICHER UN MESSAGE TEMPORAIRE (TOAST)
            vm.ToastRequested += ShowToast;
        }

        /// <summary>
        /// MISE À JOUR DE LA COULEUR DU STROKE SELON LE THÈME DE N.I.N.A.
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e) {
            if (ReferenceTextBlock.Foreground is SolidColorBrush textBrush) {
                // COULEUR DU TEXTE DE N.I.N.A.
                this.Resources["FilterWheelStrokeBrush"] = new SolidColorBrush(textBrush.Color);
            } else {
                // COULEUR PAR DÉFAUT SI NON DISPONIBLE
                this.Resources["FilterWheelStrokeBrush"] = new SolidColorBrush(Colors.Black);
            }
        }

        /// <summary>
        /// LANCE LE CALCUL DE PLANIFICATION ET MET À JOUR LA ROUE DES FILTRES
        /// </summary>
        private void OnCalculateClicked(object sender, RoutedEventArgs e) {
            if (DataContext is PlanMyNightDockableViewModel vm) {
                vm.CalculateResults();
                vm.UpdateFilterSegments();
            }
        }

        /// <summary>
        /// CHARGEMENT D’UN PROFIL ENREGISTRÉ
        /// </summary>
        private void OnLoadProfileClicked(object sender, RoutedEventArgs e) {
            if (DataContext is PlanMyNightDockableViewModel vm) {
                vm.OnLoadProfileClicked();
            }
        }

        /// <summary>
        /// SAUVEGARDE DU PROFIL ACTUEL
        /// </summary>
        private void OnSaveProfileClicked(object sender, RoutedEventArgs e) {
            if (DataContext is PlanMyNightDockableViewModel vm) {
                vm.OnSaveProfileClicked();
            }
        }

        /// <summary>
        /// SUPPRESSION D’UN PROFIL
        /// </summary>
        private void OnDeleteProfileClicked(object sender, RoutedEventArgs e) {
            if (DataContext is PlanMyNightDockableViewModel vm) {
                vm.OnDeleteProfileClicked();
            }
        }

        /// <summary>
        /// AFFICHE UN MESSAGE TEMPORAIRE (TOAST) PENDANT 2 SECONDES
        /// </summary>
        private async void ShowToast(string message) {
            ToastText.Text = message;
            ToastNotification.Visibility = Visibility.Visible;

            await Task.Delay(2000);

            ToastNotification.Visibility = Visibility.Collapsed;
        }

    }
}
