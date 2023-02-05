namespace EasyWord.Models
{
    public class OptionsChangedEventArgs
    {
        public OptionsChangedEventArgs(bool isCaseSensitive, string lernSprache)
        {
            this.IsCaseSensitive = isCaseSensitive;
            this.LernSprache = lernSprache;
        }

        public bool IsCaseSensitive { get; set; }

        public string LernSprache { get; set; }
    }
}
