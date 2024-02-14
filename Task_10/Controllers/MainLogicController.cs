using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Tracing;
using System.Net;
using System.Text.RegularExpressions;
using Task_10.Models;
using Task_10.SortingAlgorithms;

namespace Task_10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainLogicController : ControllerBase
    {
        private static readonly IConfiguration jsonConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(jsonConfig.GetSection("Settings:ParallelLimit").Get<int>());

        [HttpGet]
        public ActionResult Get(string inputLine, int sortOption)
        {
            if (!semaphore.Wait(0))
            {
                return StatusCode(503, "Слишком много запросов на сервер!");
            }

            try
            {
                string badRequestLine;

                ResultData resultData = new ResultData();

                //Checking concurrent requests
                //Thread.Sleep(5000);

                if (jsonConfig.GetSection("Settings:BlackList").Get<string[]>().Any(str => str == inputLine))
                {
                    badRequestLine = $"'{inputLine}' находится в черном списке: ";
                    foreach (string str in jsonConfig.GetSection("Settings:BlackList").Get<string[]>())
                    {
                        badRequestLine += str + ", ";
                    }
                    return BadRequest(badRequestLine);
                }

                badRequestLine = LogicTask2(inputLine);

                if (badRequestLine != null)
                {
                    return BadRequest("В строке должны быть только латинские буквы в нижнем регистре! Неподходящие символы: "
                        + badRequestLine);
                }

                string resultLine = LogicTask1(inputLine);
                resultData.ResultLine = resultLine;

                resultData.CharsAmounts = LogicTask3(resultLine);

                resultData.LongSubline = LogicTask4(resultLine);

                if (sortOption != 1 && sortOption != 2)
                {
                    return BadRequest("1 - quick sort; 2 - tree sort!");
                }


                resultData.SortResultLine = LogicTask5(resultLine, sortOption);

                int delIndex;
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(jsonConfig.GetSection("RandomApi").Get<string>() + $"?min=0&max={resultLine.Length}&count=1");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        delIndex = (streamReader.ReadToEnd()[1] - '0');
                    }
                }
                catch (WebException)
                {
                    Random rnd = new Random();
                    delIndex = rnd.Next(0, resultLine.Length);
                }

                resultData.ReduceLine = resultLine.Remove(delIndex, 1);

                return Ok(resultData);
            }
            finally 
            {
                semaphore.Release();
            }
        }
        [NonAction]
        public string LogicTask1(string inputLine)
        {
            string resultLine;

            char[] mainLine = inputLine.ToCharArray();
            Array.Reverse(mainLine);

            if (mainLine.Length % 2 != 0)
            {
                resultLine = new string(mainLine);
                Array.Reverse(mainLine);
                resultLine += new string(mainLine);

            }
            else
            {
                var lastSegment = new ArraySegment<char>(mainLine, 0, mainLine.Length / 2);
                var firstSegment = new ArraySegment<char>(mainLine, mainLine.Length / 2, mainLine.Length / 2);
                resultLine = String.Join("", firstSegment) + (String.Join("", lastSegment));
            }

            return resultLine;
        }

        [NonAction]
        public string LogicTask2(string inputLine)
        {
            if (!Regex.IsMatch(inputLine, "^[a-z]+$"))
            {
                if (Regex.Replace(inputLine, " ", "") != "")
                {
                    return Regex.Replace(Regex.Replace(inputLine, "[a-z]", ""), " ", " 'Пробел' ");
                }
                else
                {
                    return "";
                };
            }
            return null;
        }

        [NonAction]
        public Dictionary<char, int> LogicTask3(string resultLine)
        {
            Dictionary<char, int> resultDict = new Dictionary<char, int>();
            foreach (char letter in resultLine)
            {
                resultDict[letter] = resultLine.Count(lt => lt == letter);
            }
            return resultDict;
        }

        [NonAction]
        public string LogicTask4(string resultLine)
        {
            string maxLine = "";

            foreach (Match match in Regex.Matches(resultLine, "[aeiouy].*[aeiouy]"))
            {
                if (maxLine.Length < match.Value.Length)
                {
                    maxLine = match.Value;
                }
            }

            return maxLine;
        }

        [NonAction]
        public string LogicTask5(string resultLine, int sortOption)
        {
            Quicksort quicksort = new Quicksort();
            Treesort treesort = new Treesort();

            char[] resultLineChars = resultLine.ToCharArray();

            if (sortOption == 1)
            {
                return new string(quicksort.QuicksortLogic(resultLineChars, 0, resultLineChars.Length - 1));
            }
            else
            {
                return new string(treesort.TreeSort(resultLineChars));
            }
        }
    }
}
