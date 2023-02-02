using FSTools;
namespace FSLibrary
{
    public class FSFile
    {
        private FSList<byte> Data;

        public bool IsDirectory;

        public FSFile()
        {
            Data = new FSList<byte>();
            IsDirectory = false;
        }

        public static void DisplayFileContent(FSList<byte> data)
        {
            foreach (var b in data)
            {
                Console.Write((char)b);
            }
        }
    }
}
