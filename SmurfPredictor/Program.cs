using SmurfPredictorModelTraining;

namespace SmurfPredictor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Example of using smur predictor
            SmurfPredictor smurfPredictor = new SmurfPredictor();

            // Populate with account data from steam api calls
            AccountDataSchema testAccount = new AccountDataSchema
            {
                GamesOwned = 2,
                TotalPlaytime = 150,
                AccountLifetime = 600,
                RecentPlaytime = 0,
            };

            AccountDataSchema testAccount2 = new AccountDataSchema
            {
                GamesOwned = 40,
                TotalPlaytime = 600,
                AccountLifetime = 900,
                RecentPlaytime = 15,
            };

            // Predict
            AccountPrediction prediction = smurfPredictor.Predict(testAccount);
            AccountPrediction prediction1 = smurfPredictor.Predict(testAccount2);

            // Likelihood needs to be added
            Console.WriteLine(prediction.IsSmurf);
            Console.WriteLine(prediction1.IsSmurf);
        }
    }
}