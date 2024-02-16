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
            // Not pretty at all
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string assemblyParent = Directory.GetParent(assemblyPath).FullName;
            string assemblyParentParent = Path.GetDirectoryName(assemblyParent);
            string assemblyParentParentParent = Path.GetDirectoryName(assemblyParentParent);
            Console.WriteLine(assemblyParentParentParent);

            // put the csv in this folder /SteamPlayerInvestigator/DataCollection/LabelledAccount.csv

            string configFile = Path.Combine(assemblyParentParentParent, @"config.cfg"); ;
            string labelledAccountsDataFile = Path.Combine(assemblyParentParentParent, @"LabelledAccountData.csv"); ;

            LabelledAccountsDataExporter dataExporter = new LabelledAccountsDataExporter(configFile, labelledAccountsDataFile);
            dataExporter.ExportLabelledAccountsData();
        }
    }
}