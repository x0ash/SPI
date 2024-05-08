using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Text;

// Start the server
NamedPipeServerStream pipeServer;
StreamWriter sw;
StreamReader sr;

HttpClient httpClient = new HttpClient();

pipeServer = new NamedPipeServerStream("spi", PipeDirection.InOut);
sw = new StreamWriter(pipeServer);
sr = new StreamReader(pipeServer);

Process clientProc;
bool connected = false;

try
{
    string url = "";
    while (true)
    {
        if (connected && sr.Peek() > 0)
        {
            url = sr.ReadLine();
            Console.WriteLine("URL: {0}", url);
            if (url.Substring(0,10) == "spi::pid::")
            {
                int pid = int.Parse(url.Substring(10));
                Console.WriteLine("Client PID: {0}", pid);
                clientProc = Process.GetProcessById(pid);
                clientProc.EnableRaisingEvents = true;
                clientProc.Exited += CloseServer;
                sw.WriteLine("spi::status::0");
                pipeServer.Close();
                connected = false;
            }
            else
            {
                string page = GetHTMLPage(url);
                sw.Write(page);
                Console.WriteLine(page);
            }
        }
        else
        {
            pipeServer.Close();
            pipeServer = new NamedPipeServerStream("spi", PipeDirection.InOut);
            sw = new StreamWriter(pipeServer);
            sr = new StreamReader(pipeServer);
            Console.WriteLine("Waiting on client connection..");
            pipeServer.WaitForConnection();
            sw.AutoFlush = true;
            Console.WriteLine("Client connected!");
            connected = true;
        }
    }
}
catch (IOException e)
{
    Console.WriteLine("Error: {0}", e.Message);
}

pipeServer.Close();
Console.ReadLine();

async Task<string> RequestHTMLPage(string url)
{
    try
    {
        return await httpClient.GetStringAsync(url);
    }

    catch (Exception ex)
    {
        Console.WriteLine(ex);
        return "-1";
    }
}
string GetHTMLPage(string url)
{
    return RequestHTMLPage(url).Result;
}

void CloseServer(object sender, EventArgs e)
{
    pipeServer.Close();
    System.Environment.Exit(0);
}