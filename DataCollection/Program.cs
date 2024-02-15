namespace DataCollection
{
    internal class Program
    {
        // Take some labelled list of steam accounts (names or ids)
        // Use steamapi to get data needed for SmurfPredictorModel training
        // Put it all into a csv or other file for training.
        static void Main(string[] args)
        {
            Console.WriteLine("Input steam api key: ");
            string apiKey = Console.ReadLine();

            // Set this to the location of your local steam player spreadsheet.csv
            string labelledAccountsFile = "";
            string labelledAccountsDataFile = "";

            LabelledAccountsDataExporter dataExporter = new LabelledAccountsDataExporter(apiKey,labelledAccountsFile,labelledAccountsDataFile);
            dataExporter.ExportLabelledAccountsData();
        }
    }
}