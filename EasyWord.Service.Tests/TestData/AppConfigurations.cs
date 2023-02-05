namespace EasyWord.Service.Tests.TestData
{
    public class AppConfigurations
    {
        public AppConfigurations(List<string> words, bool isCaseSensitive = false, string lernLanguage = "Englisch")
        {
            this.Words = words;
            this.IsCaseSensitive = isCaseSensitive;
            this.LernLanguage = lernLanguage;
        }

        public List<string> Words { get; set; }

        public bool IsCaseSensitive { get; set; }

        public string LernLanguage { get; set; }


        public int Position { get; set; } = 0;

        public bool ReplaceWords { get; set; }
    }
}
