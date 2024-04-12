using Microsoft.ML;

namespace SmurfPredictorModelTraining
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Trains and saves model
            SmurfPredictorModelBuilder builder = new SmurfPredictorModelBuilder();

            MLContext mlContext = new MLContext();

            // Temporary example, belongs in its own class
            //Load model
            ITransformer model;
            using (FileStream fs = new FileStream("smurfPredictorModel.zip", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                model = mlContext.Model.Load(fs, out var schema);
            }

            // Create prediction engine
            var predictionEngine = mlContext.Model.CreatePredictionEngine<AccountDataSchema, AccountPrediction>(model);

            AccountDataSchema testAccount = new AccountDataSchema
            {
                GamesOwned = 2,
                TotalPlaytime = 150,
                AccountLifetime = 600,
                RecentPlaytime = 0,
            };

            var prediction = predictionEngine.Predict(testAccount);

            Console.WriteLine(prediction.IsSmurf);

        }
    }
}