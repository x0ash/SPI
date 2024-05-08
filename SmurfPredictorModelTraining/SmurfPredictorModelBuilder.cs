using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using SmurfPredictor;
using static Microsoft.ML.DataOperationsCatalog;

namespace SmurfPredictorModelTraining
{
    
    internal class SmurfPredictorModelBuilder
    {
        MLContext _mlContext;
        public SmurfPredictorModelBuilder()
        {
            _mlContext = new MLContext();

            // Split data into training and testing
            var data = LoadDataSetWithSplit(0.3f);
            var trainingData = data.TrainSet;
            var testData = data.TestSet;

            // Convert fields into floats
            // Add them to the pipeline
            // use LdSvm trainer
            var ldSvmPipeline = _mlContext.Transforms.Conversion.ConvertType("S_GamesOwned", "GamesOwned", Microsoft.ML.Data.DataKind.Single)
                .Append(_mlContext.Transforms.Conversion.ConvertType("S_AccountLifetime", "AccountLifetime", Microsoft.ML.Data.DataKind.Single))
                .Append(_mlContext.Transforms.Concatenate("Features", "S_GamesOwned", "S_AccountLifetime", "TotalPlaytime", "RecentPlaytime"))
                .Append(_mlContext.BinaryClassification.Trainers.LdSvm(labelColumnName: "IsSmurf"));

            // FastTree model
            var fastTreePipeline = _mlContext.Transforms.Conversion.ConvertType("S_GamesOwned", "GamesOwned", Microsoft.ML.Data.DataKind.Single)
                .Append(_mlContext.Transforms.Conversion.ConvertType("S_AccountLifetime", "AccountLifetime", Microsoft.ML.Data.DataKind.Single))
                .Append(_mlContext.Transforms.Concatenate("Features", "S_GamesOwned", "S_AccountLifetime", "TotalPlaytime", "RecentPlaytime"))
                .Append(_mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: "IsSmurf"));

            var fastTreeNoTimePipeline = _mlContext.Transforms.Conversion.ConvertType("S_GamesOwned", "GamesOwned", Microsoft.ML.Data.DataKind.Single)
                .Append(_mlContext.Transforms.Conversion.ConvertType("S_AccountLifetime", "AccountLifetime", Microsoft.ML.Data.DataKind.Single))
                .Append(_mlContext.Transforms.Concatenate("Features", "S_GamesOwned", "S_AccountLifetime"))
                .Append(_mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: "IsSmurf"));

            // Potentially try out lightGbm model

            //var SmurfDetectorPipeline = mlContext.Transforms.Concatenate("Features", "GamesOwned", "TotalPlaytime","AccountLifetime", "RecentPlaytime")
            //.Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "IsSmurf" ));

            // Trains the models with the data
            var ldSvmModel = ldSvmPipeline.Fit(trainingData);

            var fastTreeModel = fastTreePipeline.Fit(trainingData);

            var fastTreeNoTimeModel = fastTreeNoTimePipeline.Fit(trainingData);

            Console.WriteLine("Fitted dataview");

            using (FileStream fs = new FileStream("ldsvmSmurfPredictorModel.zip", FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                _mlContext.Model.Save(ldSvmModel, trainingData.Schema, fs);
            }

            using (FileStream fs = new FileStream("ftSmurfPredictorModel.zip", FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                _mlContext.Model.Save(fastTreeModel, trainingData.Schema, fs);
            }

            using (FileStream fs = new FileStream("ftNoTimeSmurfPredictorModel.zip", FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                _mlContext.Model.Save(fastTreeNoTimeModel, trainingData.Schema, fs);
            }

            Console.WriteLine("Saved models");
            // Test models

            // Use the model to make predictions on test data
            var predictions = fastTreeModel.Transform(testData);

            var confusionMatrix = _mlContext.BinaryClassification.Evaluate(predictions, labelColumnName: "IsSmurf").ConfusionMatrix;
            Console.WriteLine("Fast tree\n" + confusionMatrix.GetFormattedConfusionTable());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testSplit"> Percentage of data that is for testing </param>
        /// <returns></returns>
        private TrainTestData LoadDataSetWithSplit(float testSplit)
        {
            string solutionPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.Parent.FullName;
            string dataLocation = Path.Combine(solutionPath, @"DataCollection\LabelledAccountData.csv"); ;

            var data = _mlContext.Data.LoadFromTextFile<AccountDataSchema>(dataLocation, separatorChar: ',');
            Random rand = new Random();
            var splitData = _mlContext.Data.TrainTestSplit(data, testFraction: testSplit, seed: 1);
            return splitData;
        }
    }
}
