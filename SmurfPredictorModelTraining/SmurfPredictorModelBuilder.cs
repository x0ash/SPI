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

            //

            var SmurfDetectorPipeline = mlContext.Transforms.Concatenate("Features", "GamesOwned", "TotalPlaytime","AccountLifetime", "RecentPlaytime")
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "IsSmurf" ));
            
            
            // Trains the model with the data
            var SmurfDetectorModel = SmurfDetectorPipeline.Fit(DataView);
            // Makes predictions with the data
            var SmurfDetectorPredictor = SmurfDetectorModel.Transform(DataView);
            var SmurfDetectorMetrics = mlContext.BinaryClassification.Evaluate(SmurfDetectorPredictor);

            Console.WriteLine($"Accuracy: {SmurfDetectorMetrics.Accuracy:P2}");
        }
    }
}
