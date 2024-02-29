using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfPredictorModelTraining
{
    internal class SmurfPredictorModelBuilder
    {
        public SmurfPredictorModelBuilder()
        {
        var MLDetector = new MLContext();
        var DataLocation = Path.Combine(Environment.CurrentDirectory, "Data Collection" , "LabelledAccountsWithData.csv");
        var DataView = mlContext.Data.LoadFromTextFile<AccountDataSchema>(DataLocation, separator: ", ");
        var SmurfDetectorPipeline = mlContext.Transforms.Concatenate("Features", "GamesOwned", "TotalPlaytime",
"AccountLifetime")
        .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());

        var SmurfDetectorModel = SmurfDetectorPipeline.Fit(DataView);
        var SmurfDetectorPredictor = SmurfDetectorModel.Transform(DataView);
        var SmurfDetectorMetrics = mlContext.BinaryClassification.Evaluate(SmurfDetectorPredictor);

        Console.WriteLine($"Accuracy: {SmurfDetectorMetrics.Accuracy:P2}");
        }
    }
}
