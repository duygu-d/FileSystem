namespace FSLibrary
{
    [Serializable]
    public class FSDirectory : FSFile
    {
        public FSDictionary<int, string> FileAllocationTable;

        public FSDirectory()
        {
            FileAllocationTable = new FSDictionary<int, string>();
            IsDirectory = true;
        }
    }
}
