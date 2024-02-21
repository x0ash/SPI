using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollection
{
    [Serializable]
    internal struct Config
    {
        public string key { get; set; }
        public string sheetsid { get; set; }
        public string sheetsgid { get; set; }

        public Config()
        {
            key = "";
            sheetsid = "";
            sheetsgid = "";
        }
    }
}
