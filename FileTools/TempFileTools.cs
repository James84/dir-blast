using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileTools
{
    public class TempFileTools
    {
        public static string Create()
        {
            var fileName = string.Empty;

            try
            {
                fileName = Path.GetTempFileName();

                var fileInfo = new FileInfo(fileName);

                //this tells .NET this file is temporary and optimises it
                fileInfo.Attributes = FileAttributes.Temporary;

                Console.WriteLine("TEMP file created at: " + fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to create TEMP file or set its attributes: " + ex.Message);
            }

            return fileName;
        }

        public static void UpdateTmpFile(string tmpFile, string text)
        {
            try
            {
                if (File.Exists(tmpFile))
                {
                    using (var streamWriter = new StreamWriter(tmpFile))
                    {
                        streamWriter.WriteLine(text);
                        streamWriter.Flush();
                    }
                }
                else
                    Console.WriteLine("ERROR - File does not exist");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR writing to TEMP file: " + ex.Message);
            }

        }

        public static void ReadTempFile(string tmpFile)
        {
            if (File.Exists(tmpFile))
            {
                try
                {
                    using (var streamReader = File.OpenText(tmpFile))
                    {
                        Console.WriteLine("TEMP file content: " + streamReader.ReadToEnd());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading TEMP file: " + ex.Message);
                }
            }
        }

        //This will crawl through all the files and subdirectories in the  
        //path and delete files which haven't been modified for n days
        public static void Clean(string path = "", int DaysSinceLastMod = 7)
        {
            if (string.IsNullOrEmpty(path))
                path = Path.GetTempPath();

            var files = Directory.GetFiles(path);

            if (files.Any())
                DeleteFiles(files, DaysSinceLastMod);

            var directories = Directory.GetDirectories(path);

            if (directories.Any())
                foreach (var dir in directories)
                {
                    //crawl through each directory
                    //recursively and clean its files
                    Clean(dir);

                    //if directory is now empty, delete it
                    if (!Directory.GetFiles(dir).Any() && !Directory.GetDirectories(dir).Any())
                    {
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
