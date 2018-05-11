using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiscHelper
{
    [Serializable]
    public class TestData
    {
        public TestData(String sample)
        {
            Data = sample;
        }
        public String Data { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
           
        }

    }
}
