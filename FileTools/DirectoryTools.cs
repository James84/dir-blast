using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileTools
{
    public class DirectoryTools
    {
        private List<string> matchedFiles;

        public List<string> Crawl(string path, string match)
        {
            matchedFiles = new List<string>();

            CrawlDirs(path, match);

            return matchedFiles;
        }

        private void CrawlDirs(string path, string match)
        {
            //get files
            var files = Directory.GetFiles(path);

            //read the files
            if (files.Any())
                ReadFiles(files, match);

            //get subdirectories
            var directories = Directory.GetDirectories(path);

            if (directories.Any())
                foreach (var dir in directories)
                    //repeat process recursively
                    CrawlDirs(dir, match);
        }

        private void ReadFiles(string[] files, string match)
        {
            foreach (var file in files)
            {
                try
                {
                    var text = File.ReadAllText(file);

                    if (!string.IsNullOrEmpty(text) && Regex.IsMatch(text, match, RegexOptions.IgnoreCase))
                        matchedFiles.Add(file);
                }
                catch
                {
                    Console.WriteLine("Error in file: {0}", file);
                }
            }
        }
    }
}
