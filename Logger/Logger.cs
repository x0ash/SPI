using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Logger
{
    internal class Logger
    {
        private static Logger _instance = null;

        private static bool _shouldLog;

        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Logger();
                return _instance;
            }
        }

        private Logger()
        {
            //LoadConfig();
        }

        private void LoadConfig(string configFile)
        {
            using (StreamReader sr = new StreamReader(configFile))
            {
                string file = sr.ReadToEnd();
                JsonElement config = JsonSerializer.Deserialize<JsonElement>(file);
                _shouldLog = config.GetProperty("shouldLog").GetBoolean();
            }
        }
    }
}
