using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;

namespace SmurfPredictor
{
    public class SmurfPredictor
    {
        private MLContext _mlContext;
        private PredictionEngine<AccountDataSchema, AccountPrediction> _predictionEngine;
        public SmurfPredictor()
        {
            _mlContext = new MLContext();

            ITransformer model;
            // Load from file the model
            // Copy the model produced from training into the location of the assembly
            // Choose between using the ldsvm or fast tree model

            // Note that the ldsvm will not give a score, fast tree will
            using (FileStream fs = new FileStream("smurfPredictorModel.zip", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                model = _mlContext.Model.Load(fs, out var schema);
            }
            
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<AccountDataSchema, AccountPrediction>(model);
        }

        public AccountPrediction Predict(AccountDataSchema account)
        {
            AccountPrediction prediction;
            prediction = _predictionEngine.Predict(account);

            return prediction;
        }
    }
}
