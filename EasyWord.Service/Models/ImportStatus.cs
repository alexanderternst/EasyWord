namespace EasyWord.Service.Models
{
    public class ImportStatusViewModel
    {
        public ImportStatusViewModel()
        {
            this.Imported = 0;
            this.Ignored = 0;
            this.ErrorRowMessage = string.Empty;
        }

        public int Imported { get; set; }
        public int Ignored { get; set; }

        public string ErrorRowMessage { get; set; }

        public bool IsSuccessfull { get; set; } = true;

        public string ErrorMessage { get; set; } = string.Empty;
    }
}
