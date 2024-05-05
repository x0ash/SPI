using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;

namespace SmurfPredictorModelTraining
{
    internal class SmurfPredictorModelBuilder
    {
        public SmurfPredictorModelBuilder()
        {
            var mlContext = new MLContext();

            string solutionPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.Parent.FullName;
            string dataLocation = Path.Combine(solutionPath, @"DataCollection\LabelledAccountData.csv"); ;
            
            var DataView = mlContext.Data.LoadFromTextFile<AccountDataSchema>(dataLocation, separatorChar: ',');
            // Convert fields into floats
            // Add them to the pipeline
            // use LdSvm trainer
            var SmurfDetectorPipeline = mlContext.Transforms.Conversion.ConvertType("S_GamesOwned", "GamesOwned", Microsoft.ML.Data.DataKind.Single)
                .Append(mlContext.Transforms.Conversion.ConvertType("S_AccountLifetime", "AccountLifetime", Microsoft.ML.Data.DataKind.Single))
                .Append(mlContext.Transforms.Concatenate("Features", "S_GamesOwned", "TotalPlaytime", "S_AccountLifetime", "RecentPlaytime"))
                .Append(mlContext.BinaryClassification.Trainers.LdSvm(labelColumnName: "IsSmurf"));

            // FastTree model
            var fastTreePipeline = mlContext.Transforms.Conversion.ConvertType("S_GamesOwned", "GamesOwned", Microsoft.ML.Data.DataKind.Single)
                .Append(mlContext.Transforms.Conversion.ConvertType("S_AccountLifetime", "AccountLifetime", Microsoft.ML.Data.DataKind.Single))
                .Append(mlContext.Transforms.Concatenate("Features", "S_GamesOwned", "TotalPlaytime", "S_AccountLifetime", "RecentPlaytime"))
                .Append(mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: "IsSmurf"));

            // Potentially try out lightGbm model

            //var SmurfDetectorPipeline = mlContext.Transforms.Concatenate("Features", "GamesOwned", "TotalPlaytime","AccountLifetime", "RecentPlaytime")
            //.Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "IsSmurf" ));


            // Trains the model with the data
            var SmurfDetectorModel = SmurfDetectorPipeline.Fit(DataView);

            var fastTreeModel = fastTreePipeline.Fit(DataView);

            Console.WriteLine("Fitted dataview");

            using (FileStream fs = new FileStream("ldsvmSmurfPredictorModel.zip", FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                mlContext.Model.Save(SmurfDetectorModel, DataView.Schema, fs);
            }

            using (FileStream fs = new FileStream("ftSmurfPredictorModel.zip", FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                mlContext.Model.Save(fastTreeModel, DataView.Schema, fs);
            }

            Console.WriteLine("Saved model");


            // Now need to make predicitons somehow?
           
            string testDataFilePath = "test_data.csv"; // need to know the name of the csv file whcih contains test data
            // Read tfrom the CSV file
            string[] lines = File.ReadAllLines(testDataFilePath);

            // will skip the first column
            foreach (string line in lines.Skip(1))
            {
                // Split the line by commas to get individual values
                string[] values = line.Split(',');

                // needs to be set like this so algorithm can read it
                int gamesOwned = int.Parse(values[0]);
                int totalPlaytime = int.Parse(values[1]);
                int accountLifetime = int.Parse(values[2]);
                int recentPlaytime = int.Parse(values[3]);


                var account = new AccountDataSchema
                {
                    GamesOwned = gamesOwned,
                    TotalPlaytime = totalPlaytime,
                    AccountLifetime = accountLifetime,
                    RecentPlaytime = recentPlaytime
                };


            }

            // Create prediction engine
            var predictionEngine = mlContext.Model.CreatePredictionEngine<AccountDataSchema, AccountPrediction>(SmurfDetectorModel);

            // Make prediction
            //AccountDataSchema testAccount = new AccountDataSchema
            AccountDataSchema testAccount = new AccountDataSchema
            {
                GamesOwned = 2,
                TotalPlaytime = 150,
                AccountLifetime = 600,
                RecentPlaytime = 0,
            };
            var prediction = predictionEngine.Predict(testAccount);

            // Interpret prediction score as percentage
            double probabilityOfSmurf = prediction.Score;

            //  ranges and corresponding interpretations
            if (probabilityOfSmurf >= 0.9)
            {
                Console.WriteLine("Prediction: 90-100 percent a smurf");
            }
            else if (probabilityOfSmurf >= 0.8)
            {
                Console.WriteLine("Prediction: 80-89 percent a smurf");
            }
            else if (probabilityOfSmurf >= 0.7)
            {
                Console.WriteLine("Prediction: 70-79 percent a smurf");
            }
            else if (probabilityOfSmurf >= 0.6)
            {
                Console.WriteLine("Prediction: 60-69 percent a smurf");
            }
            else if (probabilityOfSmurf >= 0.5)
            {
                Console.WriteLine("Prediction: 50-59 percent a smurf");
            }
            else if (probabilityOfSmurf >= 0.4)
            {
                Console.WriteLine("Prediction: 40-49 percent a smurf");
            }
            else if (probabilityOfSmurf >= 0.3)
            {
                Console.WriteLine("Prediction: 30-39 percent a smurf");
            }
            else if (probabilityOfSmurf >= 0.2)
            {
                Console.WriteLine("Prediction: 20-29 percent a smurf");
            }
            else
            {
                Console.WriteLine("Prediction: 0-19 percent a smurf (least likely)");
            }

            // Output result
            Console.WriteLine($"Predicted label: {prediction.IsSmurf}");
        }
    }
}
