using System.Globalization;

using HomeFileRenamer.Application.Exceptions;
using HomeFileRenamer.Domain.Interfaces;

namespace HomeFileRenamer.Application.Services
{
    public class FileService : IFileService
    {
        public List<string> GetFiles(string path)
        {
            string[] fileList = Directory.GetFiles(path);

            return fileList.ToList();
        }

        public List<string> GetDirectories(string path)
        {
            string[] dirList = Directory.GetDirectories(path);

            return dirList.ToList();
        }

        public string GetDirectoryForFile(string file, List<string> dirList)
        {
            string dirName = dirList.Find(t =>
            {
                var dirPart = GetLastDirectoryInPath(t);

                return GetDateFromDirectoryOrFileName(dirPart).Equals(GetDateFromDirectoryOrFileName(file));
            });

            if (dirName == null)
            {
                throw new DirectoryNotFoundForFileException($"Directory not found for file: {file}");
            }

            return GetLastDirectoryInPath(dirName);

            string GetLastDirectoryInPath(string t)
            {
                string[] dirParts = t.Split(@"\");
                string dirPart = dirParts[dirParts.Length - 1];
                return dirPart;
            }
        }

        public DateTime GetDateFromDirectoryOrFileName(string file)
        {
            var datePart = file.Split()[0];
            datePart = datePart.Substring(0, 10);
            return DateTime.Parse(datePart, CultureInfo.CurrentCulture);
        }

        public string GetDescriptionFromDirName(string dirName)
        {
            string[] parts = dirName.Split(" - ", 2, StringSplitOptions.RemoveEmptyEntries);

            return parts.Length > 0 ? parts[1] : "";
        }

        public string GetNewFileName(string file, string dir)
        {
            string extension = Path.GetExtension(file);
            string fileName = file.Substring(0, file.Length - extension.Length);
            string description = GetDescriptionFromDirName(dir);

            string newFileName = $"{fileName} - {description}{extension}";

            return newFileName;
        }

        public List<string> RenameFiles(string filePath, string dirPath)
        {
            List<string> newFiles = new List<string>();

            var fileList = GetFiles(filePath);
            var dirList = GetDirectories(dirPath);

            foreach (var file in fileList)
            {
                try
                {
                    var directory = GetDirectoryForFile(Path.GetFileName(file), dirList);
                    var newFileName = GetNewFileName(file, directory);

                    var origPath = Path.Combine(filePath, file);
                    var newPath = Path.Combine(filePath, newFileName);

                    File.Move(origPath, newPath);
                    newFiles.Add(Path.GetFileName(newPath));
                }
                catch (DirectoryNotFoundForFileException e)
                {
                    newFiles.Add(Path.GetFileName(file));
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return newFiles;
        }
    }
}
