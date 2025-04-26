// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.


using SmartFilters.SmartFiltersDockables.ViewModels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartFilters.SmartFiltersDockables.Views {

    /// <summary>
    /// CLASSE PRINCIPALE DE LA VUE UTILISATEUR POUR LE DOCKABLE SmartFilters
    /// </summary>
    public partial class SmartFiltersDockableView : UserControl {

        /// <summary>
        /// CONSTRUCTEUR : INITIALISE LA VUE, LE DATACONTEXT ET LES ÉVÉNEMENTS
        /// </summary>
        public SmartFiltersDockableView() {
            InitializeComponent();
            this.Loaded += OnLoaded;

            var vm = new SmartFiltersDockableViewModel();
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
            if (DataContext is SmartFiltersDockableViewModel vm) {
                vm.CalculateResults();
                vm.UpdateFilterSegments();
            }
        }

        /// <summary>
        /// CHARGEMENT D’UN PROFIL ENREGISTRÉ
        /// </summary>
        private void OnLoadProfileClicked(object sender, RoutedEventArgs e) {
            if (DataContext is SmartFiltersDockableViewModel vm) {
                vm.OnLoadProfileClicked();
            }
        }

        /// <summary>
        /// SAUVEGARDE DU PROFIL ACTUEL
        /// </summary>
        private void OnSaveProfileClicked(object sender, RoutedEventArgs e) {
            if (DataContext is SmartFiltersDockableViewModel vm) {
                vm.OnSaveProfileClicked();
            }
        }

        /// <summary>
        /// SUPPRESSION D’UN PROFIL
        /// </summary>
        private void OnDeleteProfileClicked(object sender, RoutedEventArgs e) {
            if (DataContext is SmartFiltersDockableViewModel vm) {
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
