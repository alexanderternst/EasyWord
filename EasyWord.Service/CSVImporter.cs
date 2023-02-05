using System.Text;
using EasyWord.Service.Models;

namespace EasyWord.Service;

public class CSVImporter
{
    public ImportStatusViewModel ImportStatus { get; private set; }

    /// <summary>
    /// Methode welche CSV Datei importiert
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    public List<string[]> ImportCSV(string filepath)
    {
        try
        {
            ImportStatus = new ImportStatusViewModel();

            var result = File.ReadAllLines(filepath, Encoding.UTF7)
                .Select(l =>
                {
                    if (l is "" or ";")
                    {
                        return new[]{"",""};
                    }

                    var a = l.Split(';', StringSplitOptions.TrimEntries);
                    return a;
                }).ToArray();

            var row = 0;

            var list = new List<string[]>();

            foreach (var word in result)
            {
                row++;
                var germanWord = word[0];
                var englischWord = word[1];

                if (string.IsNullOrWhiteSpace(germanWord) && string.IsNullOrWhiteSpace(englischWord))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(germanWord) || string.IsNullOrWhiteSpace(englischWord))
                {
                    this.WriteError(row);
                    continue;
                }

                this.ImportStatus.Imported++;
                list.Add(new[] { germanWord, englischWord });
            }

            return list;
        }
        catch (Exception e)
        {
            this.ImportStatus.IsSuccessfull = false;
            this.ImportStatus.ErrorMessage = e.Message;
            return new List<string[]>();
        }
    }

    /// <summary>
    /// Methode welche Error ausgibt
    /// </summary>
    /// <param name="row"></param>
    private void WriteError(int row)
    {
        if (this.ImportStatus.Ignored < 1)
        {
            this.ImportStatus.ErrorRowMessage += $"Fehler in Zeile: {row}";
        }
        else
        {
            this.ImportStatus.ErrorRowMessage += $", {row}";
        }
        this.ImportStatus.Ignored++;
    }
}
