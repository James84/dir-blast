using System;

namespace CleanTempFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cleaner started");
            DirectoryUtils.Clean();
            Console.WriteLine("Cleaner finished");
            Console.ReadLine();
        }
    }
}
