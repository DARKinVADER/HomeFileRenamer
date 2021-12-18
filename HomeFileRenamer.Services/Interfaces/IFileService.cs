
using System;
using System.Collections.Generic;

namespace HomeFileRenamer.Domain.Interfaces
{
    public interface IFileService
    {
        List<string> GetFiles(string path);
        List<string> RenameFiles(string filePath, string dirPath);
        List<string> RenameFiles(string filePath, string dirPath, bool testRun = true);
    }
}