using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeFileRenamer.Console
{
    public class RenamerOptions
    {
        public string SourceFilesToRename { get; set; }
        public string FoldersWithNames { get; set; }
        public bool TestRun { get; set; }
    }
}
