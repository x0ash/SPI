using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfPredictor
{
    public class AccountPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsSmurf { get; set; }

        /// <summary>
        /// Score is not normalized (not 0-1), use probability instead.
        /// </summary>
        [ColumnName("Score")]
        public float Score { get; set; }

        /// <summary>
        /// Normalized 0-1 value
        /// </summary>
        [ColumnName("Probability")]
        public float Probability { get; set; }
    }
}
