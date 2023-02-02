using FSTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSLibrary
{
    internal class Container
    {
        private const int BLOCK_SIZE = 4096;     //4 KB    
        private const long CONTAINER_SIZE = 1099511627776; //1 TB
        private const string CONTAINER_PATH = @"C:\Users\Laptop\Desktop\КУРСОВА РАБОТА\file_system_linked_list\container.bin";

        private long BLOCK_COUNT = CONTAINER_SIZE / BLOCK_SIZE;
        private bool[] _bitmap = new bool[CONTAINER_SIZE];
        private FSList<byte[]> _fileStorage = new FSList<byte[]>();
        private FSDictionary<string, int[]> _fileAllocationTable = new FSDictionary<string, int[]>();
        private Stream _stream;

        public long FreeBlocksCount;

        public Container()
        {
            var _new = !File.Exists(CONTAINER_PATH);
            _stream = new FileStream(CONTAINER_PATH, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            FreeBlocksCount = BLOCK_COUNT; //първоначално всички блокове са свободни
        }

        public void ReadFileBytes(string fileName)
        {
            if (_fileAllocationTable.ContainsKey(fileName))
            {
                int[] blocksIndexes = _fileAllocationTable[fileName];
                int startIndex = blocksIndexes[0];
                int EndIndex = blocksIndexes[1];

                using (BinaryReader reader = new BinaryReader(_stream, Encoding.Default, true))
                {
                    reader.BaseStream.Seek(EndIndex, SeekOrigin.Begin);
                    byte[] fileBytes = reader.ReadBytes(EndIndex * BLOCK_SIZE);
                    _fileStorage.Add(fileBytes);
                }
            }
            else
            {
                Console.WriteLine("The file does not exist!");
            }
        }

        public void WriteAllFileBytes(string fileName, byte[] fileBytes)
        {
            if (_fileAllocationTable.ContainsKey(fileName))
            {
                int[] blockRange = _fileAllocationTable[fileName];
                int startIndex = blockRange[0];
                int endIndex = blockRange[1];
                int blockCount = endIndex - startIndex + 1;

                using (BinaryWriter binaryWriter = new BinaryWriter(_stream, Encoding.Default, true))
                {
                    binaryWriter.Seek(startIndex * BLOCK_SIZE, SeekOrigin.Begin);
                    binaryWriter.Write(fileBytes);
                }
            }
            else
            {
                Console.WriteLine("The file does not exist!");
            }
        }

        public int AllocateBlocks(string fileName)
        {
            int startIndex = -1;

            if (_fileAllocationTable.ContainsKey(fileName))
            {
                int[] blocksIndexes = _fileAllocationTable[fileName];
                int startBlockIndex = blocksIndexes[0];
                int endBlockIndex = blocksIndexes[1];

                int size = (endBlockIndex - startBlockIndex + 1) * BLOCK_SIZE;
                if (size > FreeBlocksCount)
                {
                    return -1;
                }

                for (int i = 0; i < BLOCK_COUNT; i++)
                {
                    if (_bitmap[i] == false)
                    {
                        startIndex = i;
                        break;
                    }
                }

                int endIndex = startIndex + size - 1;
                for (int i = startIndex; i <= endIndex; i++)
                {
                    _bitmap[i] = true;
                    FreeBlocksCount--;
                }

                _fileAllocationTable[fileName] = new int[] { startIndex, endIndex };

            }

            return startIndex;
        }

        public void UpdateFreeBlockCount()
        {
            FreeBlocksCount = 0;
            for (int i = 0; i < BLOCK_COUNT; i++)
            {
                if (!_bitmap[i])
                {
                    FreeBlocksCount++;
                }
            }
        }
    }
}
