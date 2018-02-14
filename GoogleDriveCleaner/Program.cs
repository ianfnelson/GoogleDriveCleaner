using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            var cleaner = new Cleaner();
            cleaner.Clean(args[0]);

            Console.ReadKey();
        }
    }

    public class Cleaner
    {
        public long BytesCleaned;
        public int FilesCleaned;

        public void Clean(string path)
        {
            CleanDir(path);

            Console.WriteLine("Removed " + FilesCleaned + " files");
            Console.WriteLine("Removed " + BytesCleaned + " bytes");
        }

        public void CleanDir(string path)
        {
            var di = new System.IO.DirectoryInfo(path);

            var allFiles = di.GetFiles();
            var possibleDuplicates = di.GetFiles("* (1)*");

            foreach(var possibleDuplicate in possibleDuplicates)
            {
                var originalName = possibleDuplicate.Name.Replace(" (1)", "");
                var original = allFiles.SingleOrDefault(x => x.Name == originalName);
                if (original != null && original.Length == possibleDuplicate.Length)
                {
                    BytesCleaned += possibleDuplicate.Length;
                    FilesCleaned++;
                    possibleDuplicate.Delete();
                    Console.WriteLine("Deleted " + possibleDuplicate.FullName);
                }
            }

            var subdirectories = di.GetDirectories();
            foreach (var subdirectory in subdirectories)
            {
                CleanDir(subdirectory.FullName);
            }
        }
    }
}
