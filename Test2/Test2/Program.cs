using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Test2
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {

            if (args.Length != 2)
            {
                Console.WriteLine("Incorrect input!");
                return;
            }
            if (args[0] == "scan")
            {
                var filePath = Environment.ExpandEnvironmentVariables(args[1]);
                var response = client.GetStringAsync(@$"https://localhost:5001/Test/scan?folder={filePath}").Result;
                Console.WriteLine(response);
            }
            else if (args[0] == "status")
            {
                try
                {
                    int status = Convert.ToInt32(args[1]);
                    WebRequest request = WebRequest.Create($"https://localhost:5001/Test/status/{status}");
                    WebResponse response = await request.GetResponseAsync();
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            Console.WriteLine(reader.ReadToEnd());
                        }
                    }
                    response.Close();
                }
                catch
                {
                    Console.WriteLine("Status is not a number!");
                }
            }
        }
    }
}
