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
        public FSDictionary<long, string> FileAllocationTable;

        public FSDirectory()
        {
            FileAllocationTable = new FSDictionary<long, string>();
            IsDirectory = true;
        }
    }
}
