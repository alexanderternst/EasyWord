using System;
using EasyWord.Models;
using EasyWord.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using EasyWord.Service;
using EasyWord.Service.Models;

namespace EasyWord
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppConfigurationExporter<AppConfigurations> exporter;

        /// <summary>
        /// Global Exception Handling
        /// </summary>
        /// <param name="ex"></param>
        private void OnUnhandeldException(UnhandledExceptionEventArgs ex)
        {
            MessageBox.Show("Ein Fehler bei der Aktion ist aufgetretten\nVersuche Sie es erneut.",
                "Achtung", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        /// <summary>
        /// Konstruktor welcher bei öffnung des Programmes gestartet wird und Daten (AppConfig/Wörter) aus JSON liest
        /// </summary>
        public MainWindow()
        {
            this.exporter = new AppConfigurationExporter<AppConfigurations>();

            AppDomain.CurrentDomain.UnhandledException += (s, e) => this.OnUnhandeldException(e);

            InitializeComponent();

            var appConfigurations = this.exporter.Import();

            if (appConfigurations is null)
            {
                appConfigurations = new AppConfigurations(new List<Word>());
            }

            var context = DataContext as MainWindowViewModel;
            context.AppConfigurations = appConfigurations;
            context.OnAppConfigLoaded();
        }

        /// <summary>
        /// Methode welche auf schliesen von Fenster Export Methode aufruft 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            var context = DataContext as MainWindowViewModel;
            var appsettings = context.AppConfigurations;
            appsettings.Words = context.OriginalWords;
            this.exporter.Export(appsettings);

            base.OnClosing(e);
            Application.Current.Shutdown();
        }
    }

}
