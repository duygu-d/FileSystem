using FSTools;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FSLibrary
{
    public class FSContainer
    {
        private int BlockSize { get; init; } = 4096;     //4 KB    
        private long ContainerSize { get; init; } = 1099511627776; //1 TB
        private long BlockCount;
        private string ContainerPath;

        private long[] table;
        private FSDirectory root;
        private Stream _stream;

        public FSContainer(string path)
        {
            ContainerPath = path;

            _stream = new FileStream(ContainerPath, FileMode.Open, FileAccess.ReadWrite);

            using (BinaryReader br = new BinaryReader(_stream))
            {
                BlockSize = br.ReadInt32();
                BlockCount = br.ReadInt64();
            }

            ContainerSize = BlockSize * BlockCount;
            table = new long[BlockCount];
        }

        public FSContainer(string path, int blockSize = 4096, long containerSize = 104857600)
        {
            ContainerPath = path;
            BlockSize = blockSize;
            ContainerSize = containerSize;
            BlockCount = containerSize / blockSize;
            table = new long[BlockCount];

            _stream = new FileStream(ContainerPath, FileMode.Create, FileAccess.ReadWrite);

            using (BinaryWriter bw = new BinaryWriter(_stream))
            {
                bw.Write(BlockSize);
                bw.Write(BlockCount);
            }
        }


        #region Dir

        public static byte[] SerializeDirectory(FSDirectory dir)
        {
            IFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, dir);
                return ms.ToArray();
            }
        }

        public static FSDirectory DesirializeDirectory()
        {

            IFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                FSDirectory dir = (FSDirectory)(bf.Deserialize(ms));
                return dir;
            }
        }
        public void CreateDirectory(string fullPath)
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

        public object GetDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        public object GetDirectoryRoot(string path)
        {
            if (!Tools.PathFormatValid(path))
            {
                throw new DirectoryNotFoundException();
            }

            return Directory.GetDirectoryRoot(path);
        }

        public void RenameDirectory(string path, string newName)
        {
            if (!Tools.PathFormatValid(path))
            {
                throw new DirectoryNotFoundException();
            }

            Directory.Move(path, newName);
        }

        public void DeleteDirectory(string path)
        {
            if (!Tools.PathFormatValid(path))
            {
                throw new DirectoryNotFoundException();
            }

            Directory.Delete(path);
        }

        public FSDictionary<string, string[]> GetSubdirectoriesAndFiles(string path)
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

        public bool DirectoryExists(string path)
        {
            if (!Tools.PathFormatValid(path))
            {
                throw new DirectoryNotFoundException();
            }

            return Directory.Exists(path);
        }
        #endregion


        #region File
        public void DeleteFile(string path)
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

        public void CreateFile(string path)
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

        public void DisplayFileContent(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File does not exist!");
            }

            Console.WriteLine(File.ReadAllText(path));
        }
        #endregion

        public void ReadFileBytes(string fileName)
        {
            //if (_fileAllocationTable.ContainsKey(fileName))
            //{
            //    int[] blocksIndexes = _fileAllocationTable[fileName];
            //    int startIndex = blocksIndexes[0];
            //    int EndIndex = blocksIndexes[1];

            //    using (BinaryReader reader = new BinaryReader(_stream, Encoding.Default, true))
            //    {
            //        reader.BaseStream.Seek(EndIndex, SeekOrigin.Begin);
            //        byte[] fileBytes = reader.ReadBytes(EndIndex * BLOCK_SIZE);
            //        _fileStorage.Add(fileBytes);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("The file does not exist!");
            //}
        }

        public void WriteAllFileBytes(string fileName, byte[] fileBytes)
        {
            //if (_fileAllocationTable.ContainsKey(fileName))
            //{
            //    int[] blockRange = _fileAllocationTable[fileName];
            //    int startIndex = blockRange[0];
            //    int endIndex = blockRange[1];
            //    int blockCount = endIndex - startIndex + 1;

            //    using (BinaryWriter binaryWriter = new BinaryWriter(_stream, Encoding.Default, true))
            //    {
            //        binaryWriter.Seek(startIndex * BLOCK_SIZE, SeekOrigin.Begin);
            //        binaryWriter.Write(fileBytes);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("The file does not exist!");
            //}
        }

        public int AllocateBlocks(string fileName)
        {
            //int startIndex = -1;

            //if (_fileAllocationTable.ContainsKey(fileName))
            //{
            //    int[] blocksIndexes = _fileAllocationTable[fileName];
            //    int startBlockIndex = blocksIndexes[0];
            //    int endBlockIndex = blocksIndexes[1];

            //    int size = (endBlockIndex - startBlockIndex + 1) * BLOCK_SIZE;
            //    if (size > FreeBlocksCount)
            //    {
            //        return -1;
            //    }

            //    for (int i = 0; i < BLOCK_COUNT; i++)
            //    {
            //        if (_bitmap[i] == false)
            //        {
            //            startIndex = i;
            //            break;
            //        }
            //    }

            //    int endIndex = startIndex + size - 1;
            //    for (int i = startIndex; i <= endIndex; i++)
            //    {
            //        _bitmap[i] = true;
            //        FreeBlocksCount--;
            //    }

            //    _fileAllocationTable[fileName] = new int[] { startIndex, endIndex };

            //}

            //return startIndex;
            return 0;
        }

        public void UpdateFreeBlockCount()
        {
            //FreeBlocksCount = 0;
            //for (int i = 0; i < BLOCK_COUNT; i++)
            //{
            //    if (!_bitmap[i])
            //    {
            //        FreeBlocksCount++;
            //    }
            //}
        }
    }
}
