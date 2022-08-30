using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public TestController()
        {
        }
        /// <summary>
        /// Список с задачами.
        /// </summary>
        private static List<DataClass> stat = new List<DataClass>();
        [HttpGet]
        public string Get()
        {
            return "Hello there! I am general Kenobi.";
        }
        [HttpGet("status/{status}")]
        public string Get(int status)
        {
            if (stat.Count <= status)
            {
                return $"There is no such Id! Your Id is: {status}";
            }
            if (stat[status].Finished == false)
            {
                return "Scan task in progress, please wait";
            }
            else
            {
                return @$"====== Scan result ======

Directory: {stat[status].Path}
 Processed files: {stat[status].Amount}
JS detects: {stat[status].Mistakes.Item3}
rm - rf detects: {stat[status].Mistakes.Item2}
Rundll32 detects: {stat[status].Mistakes.Item1}
Errors: {stat[status].Mistakes.Item4}
Exection {stat[status].Interval}
=========================
";
            }
        }
        [HttpGet("scan/")]
        public string Get(string folder)
        {
            var data = new DataClass(folder);
            stat.Add(data);
            data.Id = stat.IndexOf(data);
            Thread myThread = new Thread(Scanner);
            myThread.Start(data);
            return $"Scan task was created with ID: {data.Id}";
        }
        /// <summary>
        /// Метод сканирования директорий на наличие опасных файлов.
        /// </summary>
        /// <param name="data">Экземпляр класса DataClass.</param>
        private void Scanner(object data)
        {
            DateTime first = DateTime.Now;
            var info = data as DataClass;
            var pathWithEnv = info.Path;
            var filePath = Environment.ExpandEnvironmentVariables(pathWithEnv);
            try
            {
                var root = Directory.GetFiles(filePath);
                info.Amount = root.Length;
                foreach (var file in root)
                {
                    try
                    {
                        var check = System.IO.File.ReadAllText(file);
                        if (check.Contains(@"rm -rf %userprofile%\Documents"))
                        {
                            info.Mistakes.Item2++;
                        }
                        else if (check.Contains("Rundll32 sus.dll SusEntry"))
                        {
                            info.Mistakes.Item1++;
                        }
                        else if (Path.GetExtension(file) == ".js" && check.Contains("<script>evil_script()</script>"))
                        {
                            info.Mistakes.Item3++;
                        }
                    }
                    catch
                    {
                        info.Mistakes.Item4++;
                    }
                }
            }
            catch
            {
                Console.WriteLine("Incorrect path!");
                stat.Remove(info);
            }
            DateTime second = DateTime.Now;
            info.Interval = second - first;
            info.Finished = true;
        }
    }
}
