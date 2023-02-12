# EasyWord
Eine Sprachlernen-Applikation, mit welcher man Wörter, einer anderen Sprache lernen kann. Die Applikation funktioniert, indem ein Wort auf einer Fremdsprache angezeigt wird, und man das Wort auf der nativen Sprache eingeben muss, dies kann auch umgekehrt werden (Eingabe auf Fremdsprache).  
Zusätzlich kann die Gross-/Kleinschreibung Erkennung deaktiviert werden. Wörter werden erst nach zwei korrekten Eingaben als korrekt gespeichert, Wörter können bis zu 3-mal falsch eingegeben werden und welle Wörter mindestens zweimal korrekt eingegeben wurden, wird abgefragt, ob Wörter repetiert werden sollen.  
Dieses Programm bietet auch die Funktion Wörter zu löschen, und Wörter per CSV zu importieren.
Einstellungen und Wörter werden bei schliessen/öffnen des Programmes in einer/von einer JSON Datei exportiert/importiert.  
Für die Durchsetzung dieses Programmes haben wir WPF und C# verwendet, zur Darstellung wo im Programm sich ein Wort befindet, gibt es zusätzlich eine grafische Darstellung.

## Technologien/Software

Dieses Projekt wurde auf .NET 6.0 erstellt und folgende NuGet Pakete (mit Versionen) wurden/kamen installiert:  
- LiveCharts.Wpf, Version=0.9.7
- Prism.Core, Version=8.1.97

Visual Studio 2022 ist bei erstellung dieses Projektes auf Version 17.4.4  
Auf dem Rechner, mit dem das Projekt erstellt wurde, läuft eine Version von Microsoft Windows 11
