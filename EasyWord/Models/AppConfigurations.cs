using System.Collections.Generic;

namespace EasyWord.Models
{
    public class AppConfigurations
    {
        public AppConfigurations(List<Word> words, bool isCaseSensitive = false, string lernLanguage = "Englisch")
        {
            this.Words = words;
            this.IsCaseSensitive = isCaseSensitive;
            this.LernLanguage = lernLanguage;
        }

        public List<Word> Words { get; set; }

        public bool IsCaseSensitive { get; set; }

        public string LernLanguage { get; set; }


        public int Position { get; set; }

        public bool ReplaceWords { get; set; }
    }
}
