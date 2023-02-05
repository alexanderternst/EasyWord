using EasyWord.Utility;
using System;
using System.Collections.Generic;
using EasyWord.Models;

namespace EasyWord.ViewModels
{
    public class OptionenWindowViewModel : ViewModelBase
    {
        public RelayCommand CmdSend { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public OptionenWindowViewModel()
        {
            this.CmdSend = new RelayCommand(param => OnOptionsChanged.Invoke(this, new OptionsChangedEventArgs(IsCaseSensitive, Lernsprache))
                , param => true);
        }

        #region Properties
        /// <summary>
        /// EventHandler für wenn Einstellungen geändert werden
        /// </summary>
        public event EventHandler<OptionsChangedEventArgs> OnOptionsChanged = delegate { };

        private bool isCaseSensitive;
        public bool IsCaseSensitive
        {
            get
            {
                return isCaseSensitive;
            }set
            {
                if (value != isCaseSensitive)
                {
                    this.SetProperty(ref isCaseSensitive, value);
                }
            }
        }

        private string lernsprache;
        public string Lernsprache
        {
            get
            {
                return lernsprache;
            }
            set
            {
                if (value != lernsprache)
                {
                    this.SetProperty(ref lernsprache, value);
                }
            }
        }

        public KeyValuePair<string, string>[] Lernsprachen => lernsprachen;

        private readonly KeyValuePair<string, string>[] lernsprachen =
            {
                new ("Englisch", "Englische Eingabe"),
                new ("Deutsch", "Deutsche Eingabe")
            };

        #endregion
    }
}