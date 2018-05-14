using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseCSVToJson
{
    public class Program
    {
        static void Main(string[] args)
        {

            String FileContents = File.ReadAllText(@"Input.csv"); // Read the input.csv from the output folder in bin

            Root rt = new Root(FileContents);

            String outputJson = JsonConvert.SerializeObject(rt);

            Console.WriteLine(outputJson);

            File.WriteAllText("Output.json", outputJson);

        }
    }
}
