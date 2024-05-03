using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfPredictorModelTraining
{
    public class AccountPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsSmurf { get; set; }
    }
}
