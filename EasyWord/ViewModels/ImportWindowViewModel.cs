using EasyWord.Models;
using EasyWord.Service;
using EasyWord.Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;

namespace EasyWord.ViewModels
{
    public class ImportWindowViewModel : ViewModelBase
    {
        #region Properties
        /// <summary>
        /// EventHandler für wenn Wörter importiert werden
        /// </summary>
        public event EventHandler<ImportEventArgs> OnImportWord = delegate {  };

        private string _dateipfad;
        public string Dateipfad
        {
            get { return _dateipfad; }
            set
            {
                if (value != _dateipfad)
                {
                    SetProperty(ref _dateipfad, value);
                }
            }
        }

        private bool _replaceWords;
        public bool ReplaceWords
        {
            get { return _replaceWords; }
            set
            {
                if (value != _replaceWords)
                {
                    SetProperty(ref _replaceWords, value);
                }
            }
        }

        private RelayCommand _cmdFileSelector;
        private RelayCommand _cmdFileImport;
        #endregion

        #region Command Binding Propeties
        public RelayCommand CmdFileSelector
        {
            get { return _cmdFileSelector; }
            set { _cmdFileSelector = value; }
        }

        public RelayCommand CmdFileImport
        {
            get { return _cmdFileImport; }
            set { _cmdFileImport = value; }
        }
        #endregion

        /// <summary>
        /// Konstruktor
        /// </summary>
        public ImportWindowViewModel()
        {
            _cmdFileSelector = new RelayCommand(param => Execute_FileSelector(), param => true);
            _cmdFileImport = new RelayCommand(param => Execute_FileImport(), param => CanExecute_FileImport());
        }

        #region Execute Methoden
        /// <summary>
        /// Methode welche auf Knopfdruck aufgerufen wird und Dateiauswahlsfenster öffnet
        /// </summary>
        public void Execute_FileSelector()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "CSV (Comma delimited)|*.csv";
            fileDialog.InitialDirectory = @"c:\temp\";
            fileDialog.ShowDialog();

            Dateipfad = fileDialog.FileName;
        }

        /// <summary>
        /// Methode welche auf Knopfdruck aufgerufen wird und Datei über Service importiert.
        /// Zusätzlich überprüft diese Methode ob Daten hinzugefügt oder ersetzt werden.
        /// </summary>
        public void Execute_FileImport()
        {
            var iea = new ImportEventArgs();
            iea.ImportedWords = new List<Word>();
            iea.ReplaceWords = true;
            if (ReplaceWords == true)
            {
                iea.ReplaceWords = true;
            }
            else
            {
                iea.ReplaceWords = false;
            }

            var csvImporter = new CSVImporter();
            var words = csvImporter.ImportCSV(Dateipfad);

            iea.ImportedWords.Clear();

            foreach (var word in words)
            {
                iea.ImportedWords.Add(new Word(word[0], word[1]));
            }

            if (csvImporter.ImportStatus.IsSuccessfull)
            {
                MessageBox.Show($"Importiert: {csvImporter.ImportStatus.Imported}\nIgnoriert: {csvImporter.ImportStatus.Ignored}\n{csvImporter.ImportStatus.ErrorRowMessage}"
                , "Dateiimport komplett", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Dateipfad = string.Empty;
                this.OnImportWord.Invoke(this, iea);
            }
            else
            {
                MessageBox.Show(csvImporter.ImportStatus.ErrorMessage
                    , "Fehler beim Importieren", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        #endregion

        #region CanExecute Methode
        /// <summary>
        /// Methode welche überprüft ob Import Knopf aktiv sein soll (Knopf ist aktiv wenn Dateipfad nicht leer ist)
        /// </summary>
        /// <returns></returns>
        public bool CanExecute_FileImport()
        {
            return !string.IsNullOrWhiteSpace(Dateipfad);
        }
        #endregion
    }
}
