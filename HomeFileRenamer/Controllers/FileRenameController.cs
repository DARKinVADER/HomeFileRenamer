using System.Collections.Generic;

using HomeFileRenamer.Domain.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomeFileRenamer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileRenameController : ControllerBase
    {
        private readonly ILogger<FileRenameController> _logger;
        private readonly IFileService _fileService;

        public FileRenameController(ILogger<FileRenameController> logger, IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }

        [HttpGet]
        public ActionResult<List<string>> Get(string filesPath, string directoriesPath)
        {
            List<string> newFiles = new List<string>();

            newFiles = _fileService.RenameFiles(filesPath, directoriesPath);

            return newFiles;
        }
    }
}
