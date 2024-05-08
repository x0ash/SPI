using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using static Microsoft.ML.DataOperationsCatalog;

namespace SmurfPredictorModelTraining
{
    
    internal class SmurfPredictorModelBuilder
    {
        MLContext _mlContext;
        public SmurfPredictorModelBuilder()
        {
            _mlContext = new MLContext();

            // Split 70 30 training testing data
            var data = LoadDataSetWithSplit(0.3f);
            var trainingData = data.TrainSet;
            var testData = data.TestSet;

            // Convert fields into floats
            // Add them to the pipeline
            // use LdSvm trainer
            var ldSvmPipeline = _mlContext.Transforms.Conversion.ConvertType("S_GamesOwned", "GamesOwned", Microsoft.ML.Data.DataKind.Single)
                .Append(_mlContext.Transforms.Conversion.ConvertType("S_AccountLifetime", "AccountLifetime", Microsoft.ML.Data.DataKind.Single))
                .Append(_mlContext.Transforms.Concatenate("Features", "S_GamesOwned", "TotalPlaytime", "S_AccountLifetime", "RecentPlaytime"))
                .Append(_mlContext.BinaryClassification.Trainers.LdSvm(labelColumnName: "IsSmurf"));

            // FastTree model
            var fastTreePipeline = _mlContext.Transforms.Conversion.ConvertType("S_GamesOwned", "GamesOwned", Microsoft.ML.Data.DataKind.Single)
                .Append(_mlContext.Transforms.Conversion.ConvertType("S_AccountLifetime", "AccountLifetime", Microsoft.ML.Data.DataKind.Single))
                .Append(_mlContext.Transforms.Concatenate("Features", "S_GamesOwned", "TotalPlaytime", "S_AccountLifetime", "RecentPlaytime"))
                .Append(_mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: "IsSmurf"));

            // Trains the models with the data
            var ldSvmModel = ldSvmPipeline.Fit(trainingData);

            var fastTreeModel = fastTreePipeline.Fit(trainingData);

            Console.WriteLine("Fitted dataview");

            using (FileStream fs = new FileStream("ldsvmSmurfPredictorModel.zip", FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                _mlContext.Model.Save(ldSvmModel, trainingData.Schema, fs);
            }

            using (FileStream fs = new FileStream("ftSmurfPredictorModel.zip", FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                _mlContext.Model.Save(fastTreeModel, trainingData.Schema, fs);
            }

            Console.WriteLine("Saved models");
   
            // Test models

        }

        private TrainTestData LoadDataSetWithSplit(float split)
        {
            string solutionPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.Parent.FullName;
            string dataLocation = Path.Combine(solutionPath, @"DataCollection\LabelledAccountData.csv"); ;

            var data = _mlContext.Data.LoadFromTextFile<AccountDataSchema>(dataLocation, separatorChar: ',');
            var splitData = _mlContext.Data.TrainTestSplit(data, testFraction: split);
            return splitData;
        }

    }
}
