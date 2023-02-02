using FSTools;
namespace FSLibrary
{
    [Serializable]
    public class FSFile
    {
        public FSList<byte> Data;

        public bool IsDirectory;

        public FSFile()
        {
            Data = new FSList<byte>();
            IsDirectory = false;
        }
    }
}
