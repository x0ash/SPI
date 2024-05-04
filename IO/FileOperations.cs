using System.Reflection;
using System.Text.Json;

namespace IO
{
    public static class FileOperations
    {
        public static string path = "";          // Path should be the same directory as the executable file
        public static Config loaded_config;

        public static void GetPath()
        {
            string AssemblyLocation = Assembly.GetExecutingAssembly().Location;
            if (OperatingSystem.IsWindows())
            {
                path = Directory.GetParent(AssemblyLocation).FullName;
            }

            else if (OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst())
            {
                path = Directory.GetParent(AssemblyLocation).Parent.Parent.Parent.FullName;
            }

            else if (OperatingSystem.IsLinux())
            {
                path = "Linux";
            }
            else
            {
                path = "Unknown";
            }
        }

        public static void LoadConfigFile()
        {
            using (StreamReader sr = new StreamReader(path+"/config.json"))
            {
                string file = sr.ReadToEnd();
                loaded_config = JsonSerializer.Deserialize<Config>(file);
            }
        }
    }

}
