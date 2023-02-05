using System;
using System.Collections;
using System.Collections.Generic;
using EasyWord.ViewModels;

namespace EasyWord.Models
{
    [Serializable]
    public class Word : ViewModelBase
    {
        private Bucket currentBucket = Models.Bucket.Three;

        public Bucket CurrentBucket
        {
            get { return currentBucket; }
            set
            {
                if (value != currentBucket)
                {
                    SetProperty(ref currentBucket, value);
                }
            }
        }

        public string Bucket => this.currentBucket.ToString();

        private string germanWord;

        public string GermanWord
        {
            get { return germanWord; }
            set
            {
                if (value != germanWord)
                {
                    SetProperty(ref germanWord, value);
                }
            }
        }


        private string englishWord;

        public string EnglishWord
        {
            get { return englishWord; }
            set
            {
                if (value != englishWord)
                {
                    SetProperty(ref englishWord, value);
                }
            }
        }

        public Word()
        {
            this.CurrentBucket = Models.Bucket.Three;
        }

        public Word(string germanWord, string englishWord)
            : this()
        {
            this.GermanWord = germanWord;
            this.EnglishWord = englishWord;
        }
    }
}
