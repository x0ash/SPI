using System.Reflection;

namespace DataCollection
{
    internal class Program
    {
        // Take some labelled list of steam accounts (names or ids)
        // Use steamapi to get data needed for SmurfPredictorModel training
        // Put it all into a csv or other file for training.
        static void Main(string[] args)
        {
            string projectPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.FullName;
            Console.WriteLine(projectPath);

            string configFile = Path.Combine(projectPath, @"config.json"); ;
            string labelledAccountsDataFile = Path.Combine(projectPath, @"LabelledAccountData.csv"); ;

            LabelledAccountsDataExporter dataExporter = new LabelledAccountsDataExporter(configFile, labelledAccountsDataFile);
            dataExporter.ExportLabelledAccountsData();
        }
    }
}