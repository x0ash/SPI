using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;


namespace SmurfPredictor
{
    public class AccountDataSchema
    {
        [LoadColumn(0)] public int GamesOwned { get; set; }
        [LoadColumn(1)] public float TotalPlaytime { get; set; }
        [LoadColumn(2)] public int AccountLifetime { get; set; }
        [LoadColumn(3)] public float RecentPlaytime { get; set; }

        [LoadColumn(4)] public bool IsSmurf { get; set; }

        public string ToString()
        {
            return $"{GamesOwned}, {TotalPlaytime}, {AccountLifetime}, {RecentPlaytime}";
        }
    }
}
