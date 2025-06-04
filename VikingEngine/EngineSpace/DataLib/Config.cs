using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;

namespace VikingEngine
{
    static class Config
    {
        public static string PcStoragePath = null;

        public static void OnStartUp()
        {
            // Default path: Documents\My Games\{FolderName}
            string defaultBasePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "My Games"
            );

            string userBasePath = null;

            // Config file path (next to the .exe)
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string configPath = Path.Combine(exeDir, "config.txt");

            // Load config if it exists
            if (File.Exists(configPath))
            {
                string[] lines = File.ReadAllLines(configPath);
                foreach (var line in lines)
                {
                    if (line.TrimStart().StartsWith("save_path", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = line.Split('=', 2);
                        if (parts.Length == 2)
                        {
                            string candidatePath = parts[1].Trim();
                            if (!string.IsNullOrEmpty(candidatePath) &&
                                (Directory.Exists(candidatePath) || TryCreateDirectory(candidatePath)))
                            {
                                userBasePath = candidatePath;
                            }
                        }
                    }
                }
            }
            else
            {
                // Create config file with empty save_path
                string defaultConfig =
$@"# Game Configuration
# Leave 'save_path' empty to use the default location:
# {defaultBasePath}\{FilePath.StorageFolderName}
# Changing path will disable Steam Cloud backup
save_path =
";
                File.WriteAllText(configPath, defaultConfig);
            }

            // Use either the user path or default
            string selectedBasePath = userBasePath ?? defaultBasePath;
            PcStoragePath = Path.Combine(selectedBasePath, FilePath.StorageFolderName);
        }

        private static bool TryCreateDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
