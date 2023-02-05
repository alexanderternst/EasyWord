using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using AppConfigurations = EasyWord.Service.Tests.TestData.AppConfigurations;


namespace EasyWord.Service.Tests;

public class AppConfigurationExporterTests
{
    [Test]
    public void Export_AppConfigurtionen_SerializiertDasModel()
    {
        var settings = new AppConfigurations(new List<string>()
        {
            "Hallo"
        }, true, "Englisch");

        var exporter = new AppConfigurationExporter<AppConfigurations>("testdata.json");

        exporter.Export(settings);

        var json = File.ReadAllText(exporter.Path + exporter.FileName, Encoding.UTF7);
        var result = JsonSerializer.Deserialize<AppConfigurations>(json);

        result.Should().BeEquivalentTo(settings);

    }

    [Test]
    public void Import_AppConfigurationen_DeserialisiertDasModel()
    {

        var settings = new AppConfigurations(new List<string>()
        {
            "Hallo"
        }, true, "Englisch");

        var exporter = new AppConfigurationExporter<AppConfigurations>("testdata.json");

        exporter.Export(settings);

        var result = exporter.Import();

        result.Should().BeEquivalentTo(settings);
    }

    [Test]
    public void Import_AppConfigurationenFileExistierNicht_GibNeueZurück()
    {
        var exporter = new AppConfigurationExporter<AppConfigurations>("testdata.json");

        exporter.DeleteFile();

        var result = exporter.Import();

        result.Should().BeNull();

    }
}