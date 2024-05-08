using System;
using Microsoft.ML;
using SteamAPI;
using System.Linq;

namespace SmurfPredictor
{
    public class SmurfPredictor
    {
        private MLContext _mlContext;
        private PredictionEngine<AccountDataSchema, AccountPrediction> _predictionEngine;
        private PredictionEngine<AccountDataSchema, AccountPrediction> _noTimePredictionEngine;
        public SmurfPredictor()
        {
            _mlContext = new MLContext();

            ITransformer model;
            ITransformer noTimeModel;
            // Load from file the model
            // Copy the model produced from training into the location of the assembly
            // Choose between using the ldsvm or fast tree model

            // Note that the ldsvm will not give a score, fast tree will
            using (FileStream fs = new FileStream("smurfPredictorModel.zip", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                model = _mlContext.Model.Load(fs, out var schema);
            }
            using (FileStream fs = new FileStream("ftNoTimeSmurfPredictorModel.zip", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                noTimeModel = _mlContext.Model.Load(fs, out var schema);
            }

            _predictionEngine = _mlContext.Model.CreatePredictionEngine<AccountDataSchema, AccountPrediction>(model);
            _noTimePredictionEngine = _mlContext.Model.CreatePredictionEngine<AccountDataSchema, AccountPrediction>(noTimeModel);
        }

        public AccountPrediction Predict(User user)
        {
            AccountDataSchema accountInfo = new AccountDataSchema(user);

            AccountPrediction prediction;
            if (accountInfo.TotalPlaytime == 0 || accountInfo.RecentPlaytime == 0)
            {
                prediction = _noTimePredictionEngine.Predict(accountInfo);
            }
            else
            {
                prediction = _predictionEngine.Predict(accountInfo);
            }

            
            // Additional weighting can occur here.
            // IF have 3 free games, and 2 of them have roughly equal play time

            //int countRoughlyEqualPT =
            List<Game> gamesList = user.GetGamesList().ToList();
            int sameCount = gamesList.Where(x => gamesList.Any(a => a.GetTotalPlaytimeInMinutes == x.GetTotalPlaytimeInMinutes)).Count();

            if (user.GetGameCount() == 3 && sameCount >= 2)
            {
                // Add to prediction and then normalize.
            }

            return prediction;
        }


    }
}
