using FluentAssertions;

namespace EasyWord.Service.Tests
{
    public class CSVImporterTests
    {
        [Test]
        public void ImportCSV_NoErrorsList_GibtListeZurueck()
        {
            var filepath = Environment.CurrentDirectory + "/TestData/NoErrorsList.csv";

            var testee = new CSVImporter();

            var result = testee.ImportCSV(filepath);

            result.Count.Should().Be(17);
        }

        [Test]
        public void ImportCSV_NoErrorsList_SchreibtStatusImport()
        {
            var filepath = Environment.CurrentDirectory + "/TestData/NoErrorsList.csv";

            var testee = new CSVImporter();

            testee.ImportCSV(filepath);

            testee.ImportStatus.Imported.Should().Be(17);
            testee.ImportStatus.Ignored.Should().Be(0);
            testee.ImportStatus.IsSuccessfull.Should().BeTrue();
            testee.ImportStatus.ErrorMessage.Should().BeNullOrWhiteSpace();
            testee.ImportStatus.ErrorRowMessage.Should().BeNullOrWhiteSpace();
        }


        [Test]
        public void ImportCSV_ErrorsList_GibtListeZurueck()
        {
            var filepath = Environment.CurrentDirectory + "/TestData/ErrorsList.csv";

            var testee = new CSVImporter();

            var result = testee.ImportCSV(filepath);

            result.Count.Should().Be(13);
        }

        [Test]
        public void ImportCSV_ErrorsList_SchreibtStatusImport()
        {
            var filepath = Environment.CurrentDirectory + "/TestData/ErrorsList.csv";

            var testee = new CSVImporter();

            testee.ImportCSV(filepath);

            testee.ImportStatus.Imported.Should().Be(13);
            testee.ImportStatus.Ignored.Should().Be(3);
            testee.ImportStatus.IsSuccessfull.Should().BeTrue();
            testee.ImportStatus.ErrorMessage.Should().BeNullOrWhiteSpace();
            testee.ImportStatus.ErrorRowMessage.Should().ContainAll("7", "9", "14");
        }

        [Test]
        public void ImportCSV_CsvNichtVorhanden_SchreibtStatusImport()
        {
            var filepath = Environment.CurrentDirectory + "/TestData/NonExistend.csv";

            var testee = new CSVImporter();

            testee.ImportCSV(filepath);

            testee.ImportStatus.Imported.Should().Be(0);
            testee.ImportStatus.Ignored.Should().Be(0);
            testee.ImportStatus.IsSuccessfull.Should().BeFalse();
            testee.ImportStatus.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        }

    }
}
