using EasyWord.Models;
using EasyWord.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using EasyWord.ListExtensions;
using EasyWord.Service.Models;
using LiveCharts;
using System.Threading;

namespace EasyWord.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Binding Properties

        private ChartValues<int> bucketsChart { get; set; }

        public ChartValues<int> BucketsChart
        {
            get => bucketsChart;
            set
            {
                bucketsChart = value;
                OnPropertyChanged(nameof(BucketsChart));
            }
        }

        public string[] bucketsName { get; set; }

        private string keywordLabel = string.Empty;

        public string KeywordLabel
        {
            get => keywordLabel;
            set
            {
                if (value != keywordLabel)
                {
                    SetProperty<string>(ref keywordLabel, value);
                }

            }
        }

        private string answereLabel = string.Empty;

        public string AnswereLabel
        {
            get => answereLabel;
            set
            {
                if (value != answereLabel)
                {
                    SetProperty<string>(ref answereLabel, value);
                }

            }
        }

        private string keyword = string.Empty;

        public string Keyword
        {
            get => keyword;
            set
            {
                if (value != keyword)
                {
                    SetProperty<string>(ref keyword, value);
                }

            }
        }

        private string answere = string.Empty;

        public string Answere
        {
            get => answere;
            set
            {
                if (value != answere)
                {
                    SetProperty<string>(ref answere, value);
                }

            }
        }

        #endregion

        #region Properties

        List<int> countsInBuckets = new() { 0, 0, 0, 0, 0 };
        string[] ChartColumName = { "One", "Two", "Three", "Four", "Five" };

        public string ExpectedAnswere = string.Empty;

        public Word CurrentWord { get; set; }

        public Import ImportView { get; set; }

        public OptionenWindow OptionenView { get; set; }

        //Listen für Wörter
        public List<Word> OriginalWords { get; set; } = new List<Word>(); //Liste mit Originalen Wörtern

        public AppConfigurations AppConfigurations { get; set; }

        //Binding für die Commands für WPF
        private RelayCommand _cmdCorrect;
        private RelayCommand _cmdImport;
        private RelayCommand _cmdOptions;
        private RelayCommand _cmdDeleteWordList;
        private RelayCommand _cmdInfo;
        #endregion

        #region CommandBinding Properties
        /// <summary>
        /// Command um die Liste mit zuelrenden Wörtern zu löschen
        /// </summary>
        public RelayCommand CmdDeleteWordList
        {
            get { return _cmdDeleteWordList; }
            set { _cmdDeleteWordList = value; }
        }

        /// <summary>
        /// Command um die Optionen zu öffnen
        /// </summary>
        public RelayCommand CmdOptions
        {
            get { return _cmdOptions; }
            set { _cmdOptions = value; }
        }

        /// <summary>
        /// Command für das öffnen des Fensters um CSV Dateien zu Importiern 
        /// </summary>
        public RelayCommand CmdImport
        {
            get { return _cmdImport; }
            set { _cmdImport = value; }
        }

        /// <summary>
        /// Command für button zum Korrigieren
        /// </summary>
        public RelayCommand CmdCorrect
        {
            get { return _cmdCorrect; }
            set { _cmdCorrect = value; }
        }

        /// <summary>
        /// Command um Info Fenster zu öffnen
        /// </summary>
        public RelayCommand CmdInfo
        {
            get { return _cmdInfo; }
            set { _cmdInfo = value; }
        }
        #endregion

        /// <summary>
        /// Konstuktor
        /// </summary>
        public MainWindowViewModel()
        {
            ImportView = new Import();
            ImportView.Closing += ViewOnClosing;

            _cmdCorrect = new RelayCommand(param => Execute_Correct(), param => CanExecute_Correct());
            _cmdImport = new RelayCommand(param => Execute_Import(), param => true);
            _cmdOptions = new RelayCommand(param => Execute_Options(), param => true);
            _cmdDeleteWordList = new RelayCommand(param => Execute_DeleteWordList(), param => CanExecute_DeleteWordList());
            _cmdInfo = new RelayCommand(param => Execute_Info(), param => true);

            // Eventhandler für Zugriff auf ImportWindowViewModel
            var importViewContext = this.ImportView.DataContext as ImportWindowViewModel;
            var iea = new ImportEventArgs();
            // Wenn OnImportWord in ImportWindowVieModel Invoked wird Wert in iea Objekt gespeichert und an OnImportWord Methode übergeben
            importViewContext.OnImportWord += (_, iea) => this.OnImportWord(iea);

            BucketsChart = new ChartValues<int>(countsInBuckets);
            bucketsName = ChartColumName;
        }


        #region Execute Methoden

        /// <summary>
        /// Informationen Fenster öffnen
        /// </summary>
        public void Execute_Info()
        {
            var iwin = new InfoWindow();
            iwin.ShowDialog();
        }

        /// <summary>
        /// Import Fenster öffnen
        /// </summary>
        public void Execute_Import()
        {
            this.ImportView.ShowDialog();
        }

        /// <summary>
        /// Options Fenster öffnen
        /// </summary>
        public void Execute_Options()
        {
            if (OptionenView is null)
            {
                OptionenView = new OptionenWindow(this.AppConfigurations);
                OptionenView.Closing += ViewOnClosing;
                var context = OptionenView.DataContext as OptionenWindowViewModel;
                context.OnOptionsChanged += (_, options) => OnOptionsChanged(options);
            }

            OptionenView.ShowDialog();
        }

        /// <summary>
        /// Command um die Liste der zulernenden Wörter zu löschen
        /// </summary>
        public void Execute_DeleteWordList()
        {
            OriginalWords.Clear();
            this.AppConfigurations.Position = 0;
            Keyword = "<No Words>";
            this.SetBucketsZero();
            this.SetBucketsVisual();
        }


        /// <summary>
        /// Logik für Button zum Korrigieren
        /// </summary>
        public void Execute_Correct()
        {
            if (this.OriginalWords.Count == 0)
            {
                Randomize();
            }
            else
            {

                if (this.CorrectCurrentWord())
                {
                    //Wort aus dem Balken im Balkendiagramm herausnehmen in dem er sich befinden bevor Wort richtig
                    int WordBucketNubmmerBefore = Convert.ToInt32(CurrentWord.CurrentBucket) - 1;
                    countsInBuckets[WordBucketNubmmerBefore]--;

                    this.CurrentWord.CurrentBucket++;

                    //Wort aus dem Balken im Balkendiagramm hineintun in dem er sich befinden nach Wort falsch
                    int WordBucketNubmmerAfter = Convert.ToInt32(CurrentWord.CurrentBucket) - 1;
                    countsInBuckets[WordBucketNubmmerAfter]++;

                    SetBucketsVisual();

                    MessageBox.Show("Korrekt beantwortet", "Korrekt", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (CurrentWord.CurrentBucket != Bucket.One)
                    {
                        //Wort aus dem Balken im Balkendiagramm herausnehmen in dem er sich befinden bevor Wort falsch
                        int WordBucketNubmmerBefore = Convert.ToInt32(CurrentWord.CurrentBucket) - 1;
                        countsInBuckets[WordBucketNubmmerBefore]--;

                        this.CurrentWord.CurrentBucket--;

                        //Wort aus dem Balken im Balkendiagramm hineintun in dem er sich befinden nach Wort falsch
                        int WordBucketNubmmerAfter = Convert.ToInt32(CurrentWord.CurrentBucket) - 1;
                        countsInBuckets[WordBucketNubmmerAfter]++;

                        SetBucketsVisual();

                        MessageBox.Show("Falsch beantwortet", "Inkorrekt", MessageBoxButton.OK, MessageBoxImage.Hand);
                    }
                    else
                    {
                        EveryWordWrong();
                    }
                }

                Answere = string.Empty;
                if (this.OriginalWords.CheckForAllBucketsAreFive())
                {
                    this.EveryWordRight();
                }
                else
                {
                    NextWord();
                }

            }
        }

        #endregion

        #region CanExecute Methoden
        /// <summary>
        /// Kann korrigier Button gedrückt werden?
        /// </summary>
        /// <returns>True wenn gedrückt werden kann/ Flase wenn nict gedrücktwerden kann</returns>
        public bool CanExecute_Correct()
        {
            return !string.IsNullOrWhiteSpace(Answere);
        }


        /// <summary>
        /// Kann die Liste der zulernednen Wörter gelöscht werden?
        /// </summary>
        /// <returns>True wenn Liste mit Wörtern inhalt hat, wenn kein Inalt dann false</returns>
        public bool CanExecute_DeleteWordList()
        {
            if (OriginalWords.Count == 0)
                return false;

            return true;
        }
        #endregion

        #region LogikMethoden für Wörter
        /// <summary>
        /// Erstes Wort setzen und dabei Kontrollieren ob Lîste leer ist.
        /// </summary>
        private void SetFirstWord()
        {
            if (this.OriginalWords.Count < 1)
            {
                Keyword = "<No Words>";
            }
            else
            {
                this.AppConfigurations.Position = 0;
                this.CurrentWord = this.OriginalWords[this.AppConfigurations.Position];

                this.SetCurrentWord();
            }
        }

        private void SetPositionInList(int index)
        {
            if (this.OriginalWords.Count > index)
            {
                this.AppConfigurations.Position = index;
                this.CurrentWord = this.OriginalWords[this.AppConfigurations.Position];

                this.SetCurrentWord();
            }
        }

        /// <summary>
        /// Aktuelles Wort anzeigen, je nach Sprache, Deutsch zu Englisch oder Englisch zu Deutsch
        /// </summary>
        private void SetCurrentWord()
        {
            if (CurrentWord is null)
            {
                return;
            }

            var language = this.AppConfigurations.LernLanguage;
            var englischLabel = "Englisch:";
            var germanLabel = "Deutsch:";
            var englischWord = this.CurrentWord.EnglishWord;
            var germanWord = this.CurrentWord.GermanWord;
            var currentBucket = " (In Bucket " + this.CurrentWord.CurrentBucket.ToString() + ")";

            switch (language)
            {
                case "Englisch":
                    this.KeywordLabel = germanLabel + currentBucket;
                    this.AnswereLabel = englischLabel;
                    this.Keyword = germanWord;
                    this.ExpectedAnswere = englischWord;
                    break;

                case "Deutsch":
                    this.KeywordLabel = englischLabel + currentBucket;
                    this.AnswereLabel = germanLabel;
                    this.Keyword = englischWord;
                    this.ExpectedAnswere = germanWord;
                    break;

                default:
                    this.AppConfigurations.LernLanguage = "Englisch";
                    this.SetCurrentWord();
                    break;
            }
        }

        public void NextWord()
        {
            var postition = this.AppConfigurations.Position;
            this.CurrentWord = this.OriginalWords.GetNextNonBucketFiveWord(ref postition);
            this.AppConfigurations.Position = postition;

            this.SetCurrentWord();
        }

        /// <summary>
        /// Wird ausgeführt wenn alle Wörter korrekt beantwortet wurden
        /// </summary>
        private void EveryWordRight()
        {
            var result = MessageBox.Show("Congratulations, you got everything right. Would you like to repeat all the words?", "All correct!", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                foreach (var ow in OriginalWords)
                {
                    SetBucketsZero();
                    ow.CurrentBucket = Bucket.Three;

                    GetBucketsCount();
                    SetBucketsVisual();
                }

                this.Randomize();

                this.SetFirstWord();
            }
            else
            {
                Execute_DeleteWordList();
            }

        }

        /// <summary>
        /// Wird ausgeführt wenn alle Wörter falsch beantwortet wurden
        /// </summary>
        private void EveryWordWrong()
        {
            var result = MessageBox.Show("You answered all words incorrectly. Would you like to repeat all the words?", "All wrong", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                foreach (var ow in OriginalWords)
                {
                    SetBucketsZero();
                    ow.CurrentBucket = Bucket.Three;

                    GetBucketsCount();
                    SetBucketsVisual();
                }

                this.Randomize();

                this.SetFirstWord();
            }
            else
            {
                Execute_DeleteWordList();
            }

        }

        /// <summary>
        /// Wörter aus Originaler Liste werden in einer neuen Liste zufällig angeordnet
        /// </summary>
        public void Randomize()
        {
            var rnd = new Random();
            this.OriginalWords = OriginalWords.OrderBy(item => rnd.Next()).ToList();
        }

        /// <summary>
        /// Korrigieren ob Eingabe mit dem Keyword übereinstimmt.
        /// </summary>
        /// <returns>true wenn Eingabe übereinstimmt, sonst false</returns>
        private bool CorrectCurrentWord()
        {
            var isCorrect = false;

            if (!this.AppConfigurations.IsCaseSensitive)
            {
                isCorrect = this.Answere == this.ExpectedAnswere;
            }
            else
            {
                isCorrect = string.Equals(this.Answere, this.ExpectedAnswere, StringComparison.CurrentCultureIgnoreCase);
            }

            return isCorrect;
        }
        #endregion

        #region Buckkets visualisirung 
        /// <summary>
        /// Gibt die akktuellen Werte dem Bakendiagramm so das es akktualisiert
        /// </summary>
        public void SetBucketsVisual()
        {
            BucketsChart = new ChartValues<int>(countsInBuckets);
            bucketsName = ChartColumName;
        }

        public void SetBucketsZero()
        {
            countsInBuckets.Clear();
            // Variable die zählt wieviele Wörter in Buckets werden wieder 5 leere zahlen eingefügt,
            // damit diese wieder zählen können wieviele Wörter im Bucket
            for (int i = 0; i < 5; i++)
            {
                countsInBuckets.Add(0);
            }
        }

        /// <summary>
        /// Zählt wieviele Wörter in jedem Bucket sind
        /// </summary>
        public void GetBucketsCount()
        {
            foreach (var OW in OriginalWords)
            {
                // reausfinden in welchem Bucket welches Wort ist
                var WordBucketNubmmer = (int)OW.CurrentBucket;
                WordBucketNubmmer--;
                countsInBuckets[WordBucketNubmmer]++;
            }
        }
        #endregion

        #region Zusätzliche (On-) Logikmethoden

        /// <summary>
        /// Wird ausgeführt wenn AppConfig geladen wird
        /// </summary>
        public void OnAppConfigLoaded()
        {
            this.AppConfigurations = this.AppConfigurations;
            this.OriginalWords = this.AppConfigurations?.Words ?? new List<Word>();

            this.SetPositionInList(this.AppConfigurations.Position);

            GetBucketsCount();
            SetBucketsVisual();
        }

        /// <summary>
        /// Methode die ausgeführt wird sobald Wörter importiert wurden und Wörter entweder ersetzt oder hinzugefügt (überprüfung über boolean in Objekt)
        /// Diese Methode wird durch einen EventHandler im ImportWindowViewModel ausgelöst
        /// </summary>
        /// <param name="iea">ImportEventArgs Objekt mit bool und Liste</param>
        private void OnImportWord(ImportEventArgs iea)
        {
            if (iea.ReplaceWords)
            {
                this.OriginalWords.Clear();
            }

            ImportView.Hide();
            OriginalWords.AddRange(iea.ImportedWords);
            Randomize();
            SetFirstWord();

            SetBucketsZero();

            GetBucketsCount();
            SetBucketsVisual();
        }

        /// <summary>
        /// Methode die ausgeführt wird wenn Änderungen in Einstellungen gemacht werden und diese Speichert
        /// Diese Methode wird durch einen EventHandler im OptionenWindowViewModel ausgelöst
        /// </summary>
        /// <param name="e">Optionen/Einstellungen</param>
        private void OnOptionsChanged(OptionsChangedEventArgs e)
        {
            this.AppConfigurations.IsCaseSensitive = e.IsCaseSensitive;
            this.AppConfigurations.LernLanguage = e.LernSprache;
            this.OptionenView.Hide();
            this.SetCurrentWord();
        }

        /// <summary>
        /// Methode welche ausgeführt wird wenn ImportFenster geschlossen wird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewOnClosing(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;
            var s = sender as Window;
            s.Hide();
        }
        #endregion

    }
}