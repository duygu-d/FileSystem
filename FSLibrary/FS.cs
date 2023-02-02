using FSTools;

namespace FSLibrary
{
    public static class FS
    {
        private const int bytesPerTableEntry = 4;
        public static int clusterSize; //in bytes, 4096 = 4Kb
        public static long totalSize; //in bytes, 104857600 = 100Mb
        private static long tableSizeBytes; //in bytes, 
        private static long[][] table; // [cluster number] []
        private static FileStream fs = null;

        public static void CreateFS(string path, int clusterSize = 4096, long totalSize = 104857600)
        {
            fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
            tableSizeBytes = (totalSize / clusterSize) * bytesPerTableEntry;

            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(clusterSize);
                bw.Write(tableSizeBytes);
            }
            FS.clusterSize = clusterSize;
            FS.totalSize = totalSize;
        }

        public static void LoadFS(string path)
        {
            fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            using (BinaryReader br = new BinaryReader(fs))
            {
                clusterSize = br.ReadInt32();
                tableSizeBytes = br.ReadInt64();
                totalSize = clusterSize * (tableSizeBytes / bytesPerTableEntry);
            }
        }

        #region Dir
        public static void CreateDirectory(string fullPath)
        {
            if (!Tools.PathFormatValid(fullPath))
            {
                throw new FormatException();
            }

            fullPath = fullPath.FSTrimEnd("/");
            string[] pathSplit = fullPath.FSSplit('/', StringSplitOptions.RemoveEmptyEntries);
            string destination = fullPath.FSTrimEnd(pathSplit[pathSplit.Length - 1]);

            if (destination != null && !DirectoryExists(destination))
            {
                throw new DirectoryNotFoundException();
            }


            Directory.CreateDirectory(fullPath);
        }

        public static object GetDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        public static object GetDirectoryRoot(string path)
        {
            if (!Tools.PathFormatValid(path))
            {
                throw new DirectoryNotFoundException();
            }

            return Directory.GetDirectoryRoot(path);
        }

        public static void RenameDirectory(string path, string newName)
        {
            if (!Tools.PathFormatValid(path))
            {
                throw new DirectoryNotFoundException();
            }

            Directory.Move(path, newName);
        }

        public static void DeleteDirectory(string path)
        {
            if (!Tools.PathFormatValid(path))
            {
                throw new DirectoryNotFoundException();
            }

            Directory.Delete(path);
        }

        public static FSDictionary<string, string[]> GetSubdirectoriesAndFiles(string path)
        {
            FSDictionary<string, string[]> dirsAndFiles = new FSDictionary<string, string[]>();
            string[] dirs = Directory.GetDirectories(path);

            foreach (string dir in dirs)
            {
                Directory.SetCurrentDirectory(dir);
                string[] files = Directory.GetFiles(dir);
                dirsAndFiles.Add(dir, files);
            }

            return dirsAndFiles;
        }

        public static bool DirectoryExists(string path)
        {
            if (!Tools.PathFormatValid(path))
            {
                throw new DirectoryNotFoundException();
            }

            return Directory.Exists(path);
        }
        #endregion


        #region File
        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                throw new FileNotFoundException("File does not exist!");
            }
        }

        public static void CreateFile(string path)
        {
            if (File.Exists(path))
            {
                throw new Exception("File already exists!");
            }
            else
            {
                File.Create(path);
            }
        }

        public static void DisplayFileContent(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File does not exist!");
            }

            Console.WriteLine(File.ReadAllText(path));
        }
        #endregion
    }
}