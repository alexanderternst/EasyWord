using System.Collections.Generic;

namespace EasyWord.Models
{
    public class ImportEventArgs
    {
        public List<Word> ImportedWords { get; set; }
        public bool ReplaceWords { get; set; }
    }
}