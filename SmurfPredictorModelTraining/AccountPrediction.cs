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

        // Score is not normalized
        [ColumnName("Score")]
        public float Score { get; set; }

        // Probability is normalized and therefore the more useful "certainty" value
        [ColumnName("Probability")]
        public float Probability { get; set; }
    }
}
