using FSTools;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FSLibrary
{
    public class FSContainer
    {
        public int BlockSize { get; init; } = 4096;     //4 KB    
        public long ContainerSize { get; init; } = 104857600; //100Mb
        private int BlockCount;
        private int TableSizeInBlocks { get => (int)((BlockCount * 8) / BlockSize); } // размерът на FAT в блокове: (брой блокове в контейнера * 8 байта за long) / размерът на блока
        private string ContainerPath;

        private int[] table;
        private FSDirectory root;
        private Stream _stream;
        private BinaryReader br;
        private BinaryWriter bw;

        public FSContainer(string path)
        {
            ContainerPath = path;

            _stream = new FileStream(ContainerPath, FileMode.Open, FileAccess.ReadWrite);
            InitReaderWriter();

            BlockSize = br.ReadInt32();
            BlockCount = br.ReadInt32();

            ContainerSize = BlockSize * BlockCount;

            byte[] tableData = br.ReadBytes(BlockCount * 4);

            table = tableData.FSToIntArray();

            byte[] rootData = ReadFileBytes(TableSizeInBlocks);
            root = (FSDirectory)DeserializeItem(rootData);
        }

        public FSContainer(string path, int blockSize = 4096, long containerSize = 104857600)
        {
            ContainerPath = path;
            BlockSize = blockSize;
            ContainerSize = containerSize;
            BlockCount = (int)(containerSize / blockSize);
            table = new int[BlockCount];

            for (int i = 0; i < BlockCount; i++)
            {
                table[i] = i;
            }

            _stream = new FileStream(ContainerPath, FileMode.Create, FileAccess.ReadWrite);
            InitReaderWriter();

            bw.Write(BlockSize);
            bw.Write(BlockCount);
            byte[] tableData = table.FSToByteArray();
            bw.Write(tableData);

            root = new FSDirectory();
            WriteFileBytes(SerializeItem(root), TableSizeInBlocks); // записваме root директорията веднага зад FAT
        }

        private void InitReaderWriter()
        {
            bw = new BinaryWriter(_stream);
            br = new BinaryReader(_stream);
        }

        public int AllocateBlock()
        {
            int index = TableSizeInBlocks;
            index++; //първият блок винаги е root directory, търсим след него

            while (index < table.Length)
            {
                if (table[index] == 0)
                {
                    table[index] = -1; // записваме FFFFFFFF защото блокът вече е алокиран, но още не знаем дали ще е свързан със следващ
                    return index;
                }
                index++;
            }

            throw new IndexOutOfRangeException();
        }

        public void WriteFileBytes(byte[] fileBytes, int block = -1)
        {
            if (block < 0)
            {
                block = AllocateBlock();
            }

            int bytesWritten = 0;
            bw.BaseStream.Position = block * BlockSize;

            int i = 0;
            do
            {
                bw.Write(fileBytes[i]);
                bytesWritten++;
                if (bytesWritten == BlockSize)
                {
                    int newBlock = AllocateBlock();
                    table[block] = newBlock;
                    block = newBlock;
                    bw.BaseStream.Position = block * BlockSize;
                }
                i++;
            } while (i < fileBytes.Length);
        }

        public byte[] ReadFileBytes(int block)
        {
            FSList<byte> bytes = new FSList<byte>();
            int bytesRead = 0;
            br.BaseStream.Position = block * BlockSize;
            while (true)
            {
                bytes.Add(br.ReadByte());
                bytesRead++;
                if (bytesRead == BlockSize)
                {
                    block = table[block];
                    if (block == -1)
                    {
                        break;
                    }
                    bytesRead = 0;
                    br.BaseStream.Position = block * BlockSize;
                }
            }
            return bytes.ToArray();
        }

        public void DeleteFileAt(int block)
        {
            while (true)
            {
                int nextBlock = table[block];
                if (nextBlock == -1)
                {
                    break;
                }
                table[block] = 0;
                block = nextBlock;
            }
        }

        #region Dir

        public static byte[] SerializeItem(object obj)
        {
            IFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static object DeserializeItem(byte[] data)
        {
            IFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream(data))
            {
                object dir = bf.Deserialize(ms);
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

    }
}
