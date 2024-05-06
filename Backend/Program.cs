using System;
using System.Text;
using System.IO.Pipes;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics.Eventing.Reader;

// Start the server
NamedPipeServerStream pipeServer;
StreamString ss;

HttpClient httpClient = new HttpClient();

pipeServer = new NamedPipeServerStream("spi", PipeDirection.InOut);
ss = new StreamString(pipeServer);

try
{
    bool connected = false;
    string url = "";
    while (true)
    {
        if (connected)
        {
            url = ss.ReadString();
            if (url == "spi::-1::disconnected")
            {
                //connected = false;
                System.Environment.Exit(0);
            }
            else
            {
                string page = GetHTMLPage(url);
                ss.WriteString(page);
            }
        }
        else
        {
            pipeServer.Close();
            pipeServer = new NamedPipeServerStream("spi", PipeDirection.InOut);
            ss = new StreamString(pipeServer);
            Console.WriteLine("Waiting on client connection..");
            pipeServer.WaitForConnection();
            Console.WriteLine("Client connected!");
            connected = true;
        }
    }
}
catch (IOException e)
{
    Console.WriteLine("Error: {0}",e.Message);
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

class StreamString
{
    Stream ioStream;
    UnicodeEncoding streamEncoding;

    public StreamString(Stream ioStream)
    {
        this.ioStream = ioStream;
        this.streamEncoding = new UnicodeEncoding();
    }

    public string ReadString()
    {
        try
        {
            int len = 0;

            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            byte[] inBuffer = new byte[len];
            ioStream.Read(inBuffer, 0, len);

            return streamEncoding.GetString(inBuffer);
        }

        catch
        {
            return "spi::-1::disconnected";
        }
    }

    public int WriteString(string outString)
    {
        byte[] outBuffer = streamEncoding.GetBytes(outString);
        int len = outBuffer.Length;
        if (len > UInt16.MaxValue)
        {
            len = (int)UInt16.MaxValue;
        }
        ioStream.WriteByte((byte)(len / 256));
        ioStream.WriteByte((byte)(len & 255));
        ioStream.Write(outBuffer, 0, len);
        ioStream.Flush();

        return outBuffer.Length + 2;
    }
}