
using System;
using System.Collections.Generic;

namespace HomeFileRenamer.Domain.Interfaces
{
    public interface IFileService
    {
        List<string> GetFiles(string path);
        List<string> GetDirectories(string path);
        string GetDirectoryForFile(string file, List<string> dirList);
        DateTime GetDateFromDirectoryOrFileName(string file);
        string GetDescriptionFromDirName(string dirName);
        string GetNewFileName(string file, string dir);
        List<string> RenameFiles(string filePath, string dirPath);
    }
}