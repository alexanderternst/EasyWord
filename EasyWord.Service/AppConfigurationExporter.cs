using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyWord.Service
{
    public class AppConfigurationExporter<T>
    {
        public string FileName { get; }

        public string Path { get; }

        public AppConfigurationExporter(string fileName = "appconfiguration.json")
        {
            this.Path = @$"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\EasyWord\data";
            this.FileName = fileName;
        }

        public AppConfigurationExporter(string path, string fileName)
        {
            this.Path = path;
            this.FileName = fileName;
        }

        public void DeleteFile()
        {
            File.Delete(this.Path + this.FileName );
        }

        /// <summary>
        /// Methode welche AppConfig und Wörter in JSON Datei speichert
        /// </summary>
        public void Export(T appConfig)
        {
            var json = JsonSerializer.Serialize(appConfig);

            if (!File.Exists(this.Path + this.FileName))
            {
                this.GrantAccess(this.Path);
                var reder = File.Create(this.Path + this.FileName);
                reder.Close();
            }

            File.WriteAllText(this.Path + this.FileName , json, Encoding.UTF7);
        }

        public T? Import()
        {
            try
            {
                var json = File.ReadAllText(this.Path + this.FileName, Encoding.UTF7);
                var appConfigurations = JsonSerializer.Deserialize<T>(json);
                return appConfigurations;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default(T);
            }

        }

        /// <summary>
        /// Set full Control to Everyone
        /// </summary>
        /// <param name="file">Zu welchem file</param>
        private void GrantAccess(string file)
        {
            DirectoryInfo di = System.IO.Directory.CreateDirectory(file);
            DirectoryInfo dInfo = new DirectoryInfo(file);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.Modify, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);
        }
    }
}
