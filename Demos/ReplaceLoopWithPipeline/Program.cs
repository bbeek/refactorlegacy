using System;
using System.IO;

namespace ReplaceLoopWithPipeline
{
    class Program
    {
        static void Main(string[] args)
        {
            const string fileName = "OfficeData.csv";

            using var filestream = new StreamReader(fileName);
            var norway = new OfficeReader().GetNorwayOffices(filestream.ReadToEnd());

            foreach(var office in norway)
            {
                Console.WriteLine($"Hello from {office.City} ");
            }
        }
    }
}
