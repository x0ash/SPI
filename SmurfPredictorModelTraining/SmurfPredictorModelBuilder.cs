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
        }

        static string GetTestDataFilePathFromDataCollection()
        {
            return LabelledAccountsDataExporter.GetTestDataFilePath();
        }

        static IEnumerable<AccountData> ReadTestData(string testDataFilePath)
        {
            // Read data file           
            var testData = new List<AccountData>();

            // Read data from the file
            string[] lines = File.ReadAllLines(testDataFilePath);

            // Parse each line and create AccountData objects
            foreach(var line in lines.Skip(1)) 
            {
                string[] values = line.Split(',');
                var account = new AccountData
                {
                    GamesOwned = int.Parse(values[0]),
                    TotalPlaytime = int.Parse(values[1]),
                    AccountLifetime = int.Parse(values[2]),
                    RecentPlaytime = int.Parse(values[3])
                };
                testData.Add(account);
            }

            return testData;
        }

        static void MakePredictions(MLContext mlContext, ITransformer model, IEnumerable<AccountData> testData)

            // Create prediction engine
            var predictionEngine = mlContext.Model.CreatePredictionEngine<AccountData, AccountPrediction>(SmurfPredictorModelTraining);

            // Make predictions for each test account
            foreach(var account in data)
            {
                var prediction = predictionEngine.Predict(account);

                //  prediction as percentage
                double probabilityOfSmurf = prediction.Score * 100;

                //  prediction result
                Console.WriteLine($"Prediction: {probabilityOfSmurf}% likely a smurf");

                // Output result interpretation
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

                // Output predicted label
                Console.WriteLine($"Predicted label: {prediction.Prediction}");
            }
        }
    }
    
    class DataCollection
    {
        public int GamesOwned{ get; set; }
        public int TotalPlaytime{ get; set; }
        public int AccountLifetime{ get; set; }
        public int RecentPlaytime{ get; set; }
        //prediction result   
        public bool Prediction{ get; set; }
        public float Score{ get; set; }
    }
}
