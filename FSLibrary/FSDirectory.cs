using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSLibrary
{
    [Serializable]
    public class FSDirectory : FSFile
    {
        private FSDictionary<long, string> _fileAllocationTable;

        public FSDirectory()
        {
            _fileAllocationTable = new FSDictionary<long, string>();
            IsDirectory = true;
        }
    }
}
