using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CleanTempFiles
{
    class DirectoryUtils
    {
        //This will crawl through all the files and subdirectories in the  
        //path and delete files which haven't been modified for n days
        public static void Clean(string path = "", int daysSinceLastMod = 7)
        {
            //if no path is passed in just use the temp path
            if (string.IsNullOrEmpty(path))
                path = Path.GetTempPath();

            var files = Directory.GetFiles(path);

            if (files.Any())
                DeleteFiles(files, daysSinceLastMod);

            var directories = Directory.GetDirectories(path);

            if (directories.Any())
                EnumerateDirs(directories);
        }

        private static void EnumerateDirs(IEnumerable<string> directories)
        {
            foreach (var dir in directories)
                ProcessDirectory(dir);
        }

        private static void ProcessDirectory(string dir)
        {
            //crawl through each directory
            //recursively and clean its files
            Clean(dir);

            //if directory is now empty, delete it
            if (Directory.GetFiles(dir).Any() || Directory.GetDirectories(dir).Any()) return;

            try
            {
                Directory.Delete(dir);
                Console.WriteLine("DIRECTORY AT: " + dir + " HAS BEEN DELETED");
            }
            catch
            {
                Console.WriteLine("ERROR - DIRECTORY AT: " + dir + "HAS NOT BEEN DELETED");
            }
        }

        private static void DeleteFiles(IEnumerable<string> files, int daysSinceLastMod)
        {
            foreach (var file
                        in from file
                            in files
                           let fileInfo = File.GetLastWriteTime(file)
                           where fileInfo < DateTime.Now.AddDays(-daysSinceLastMod)
                           select file)
            {
                try
                {
                    File.Delete(file);
                    Console.WriteLine("FILE DELETED AT: " + file);
                }
                catch
                {
                    Console.WriteLine("ERROR - FILE AT: " + file + " HAS NOT BEEN DELETED");
                }
            }
        }
    }
}
