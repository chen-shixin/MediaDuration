using System;
using System.IO;

namespace MediaDuration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(string.Concat("Extension\t", "Duration\t", "Milliseconds"));

            Duration durationTest = new ByFFmpeg();

            string dir = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Testcases\\");
            string[] files = Directory.GetFiles(dir);

            foreach (var filePath in files)
            {
                string extension = Path.GetExtension(filePath);
                var result = durationTest.GetDuration(filePath);

                Console.WriteLine(string.Concat(extension, ":\t\t", result.Item1, "\t", result.Item2));
            }

            Console.ReadKey();
        }
    }
}
