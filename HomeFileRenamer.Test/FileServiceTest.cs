using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using HomeFileRenamer.Application.Exceptions;
using HomeFileRenamer.Application.Services;

using Xunit;

namespace HomeFileRenamer.Test
{
    public sealed class FileServiceTest : IDisposable
    {
        private const string TestFolder = @"HomeFileRenamer";
        private readonly string TestPath;
        private FileService fileService;

        public FileServiceTest()
        {
            fileService = new FileService();

            TestPath = Path.Combine(Path.GetTempPath(), TestFolder);
            Directory.CreateDirectory(TestPath);
        }

        public void Dispose()
        {
            Directory.Delete(TestPath, true);
        }

        [Fact]
        public void FileListTest()
        {
            List<string> tmpFiles = new List<string>();

            for (int i = 0; i < 7; i++)
            {
                var file = Path.Combine(TestPath, Path.GetRandomFileName());
                tmpFiles.Add(file);
                using (FileStream fs = File.Create((file))) { }
            }

            List<string> fileList = fileService.GetFiles(TestPath);

            fileList.Sort();
            tmpFiles.Sort();

            Assert.True(fileList.SequenceEqual(tmpFiles));
        }

        [Fact]
        public void DirectoryListTest()
        {
            List<string> tmpDirectories = new List<string>();

            for (int i = 0; i < 7; i++)
            {
                var directory = Path.Combine(TestPath, Path.GetRandomFileName());
                tmpDirectories.Add(directory);
                Directory.CreateDirectory(directory);
            }

            List<string> directoryList = fileService.GetDirectories(TestPath);

            tmpDirectories.Sort();
            directoryList.Sort();

            Assert.True(directoryList.SequenceEqual(tmpDirectories));
        }

        [Fact]
        public void GetDirectoryForFileTest()
        {
            string file = "2019-07-20 10.33.55.mp4";
            List<string> dirList = new List<string>()
            {
                @"X:\Video\2019\2019.07.18-20 - Timcsi és Dórika Sződön",
                @"X:\Video\2019\2019.07.20 - Budakalász Vadaspark és Dínópark",
                @"X:\Video\2019\2019.07.21 - Dórika, Timcsike, Matyi és Máté Vácon"
            };

            string dirName = fileService.GetDirectoryForFile(file, dirList);

            Assert.Equal("2019.07.20 - Budakalász Vadaspark és Dínópark", dirName);
        }
        [Fact]
        public void GetDirectoryForFileNotFoundTest()
        {
            string file = "2019-07-27 10.33.55.mp4";
            List<string> dirList = new List<string>()
            {
                @"X:\Video\2019\2019.07.18-20 - Timcsi és Dórika Sződön",
                @"X:\Video\2019\2019.07.20 - Budakalász Vadaspark és Dínópark",
                @"X:\Video\2019\2019.07.21 - Dórika, Timcsike, Matyi és Máté Vácon"
            };

            Assert.Throws<DirectoryNotFoundForFileException>(() => fileService.GetDirectoryForFile(file, dirList));
        }

        [Fact]
        public void GetDateFromFileNameTest()
        {
            string file = "2019-07-20 10.33.55.mp4";

            DateTime fileDateTime = fileService.GetDateFromDirectoryOrFileName(file);

            Assert.True(fileDateTime.Date.Equals(new DateTime(2019, 07, 20)));
        }
        [Fact]
        public void GetDateFromDirectoryNameTest()
        {
            string file = @"2019.07.20 - Budakalász Vadaspark és Dínópark\";

            DateTime fileDateTime = fileService.GetDateFromDirectoryOrFileName(file);

            Assert.True(fileDateTime.Date.Equals(new DateTime(2019, 07, 20)));
        }
        [Fact]
        public void GetDateFromDirectoryWithDateRangeTest()
        {
            string file = @"2019.07.18-20 - Timcsi és Dórika Sződön\";

            DateTime fileDateTime = fileService.GetDateFromDirectoryOrFileName(file);

            Assert.True(fileDateTime.Date.Equals(new DateTime(2019, 07, 18)));
        }

        [Fact]
        public void GetDescriptionFromDirNameTest()
        {
            string dirName = @"2019.07.18-20 - Timcsi és Dórika Sződön";

            string description = fileService.GetDescriptionFromDirName(dirName);

            Assert.Equal(@"Timcsi és Dórika Sződön", description);
        }

        [Fact]
        public void GetNewFileNameTest()
        {
            string file = "2019-07-20 10.33.55.mp4";
            string dir = "2019.07.20 - Budakalász Vadaspark és Dínópark";

            string newFile = fileService.GetNewFileName(file, dir);

            Assert.Equal("2019-07-20 10.33.55 - Budakalász Vadaspark és Dínópark.mp4", newFile);
        }

        [Fact]
        public void RenameFilesTest()
        {
            List<string> tmpFiles = new List<string>()
            {
                "2019-07-19 18.51.07.mp4",
                "2019-07-20 10.33.55.mp4"
            };

            List<string> expectedFiles = new List<string>()
            {
                Path.Combine(TestPath, "2019-07-19 18.51.07.mp4"),
                Path.Combine(TestPath, "2019-07-20 10.33.55 - Budakalász Vadaspark és Dínópark.mp4")
            };

            foreach (var file in tmpFiles)
            {
                var filePath = Path.Combine(TestPath, file);
                using (FileStream fs = File.Create((filePath))) { }
            }

            List<string> dirList = new List<string>()
            {
                @"2019.07.18-20 - Timcsi és Dórika Sződön\",
                @"2019.07.20 - Budakalász Vadaspark és Dínópark\",
                @"2019.07.21 - Dórika, Timcsike, Matyi és Máté Vácon\"
            };

            foreach (var dir in dirList)
            {
                var dirPath = Path.Combine(TestPath, dir);
                Directory.CreateDirectory(dirPath);
            }

            fileService.RenameFiles(TestPath, TestPath);

            List<string> fileList = fileService.GetFiles(TestPath);

            Assert.Equal(2, fileList.Count);

            fileList.Sort();
            Assert.True(expectedFiles.SequenceEqual(fileList));
        }
    }
}
