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
    }
}
