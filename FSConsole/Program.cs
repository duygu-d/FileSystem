using FSLibrary;
using FSTools;

namespace FSConsole
{
    internal class Program
    {
        private static bool exiting = false;
        private static FSContainer fs;
        private static void Main(string[] args)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "fs.bin";
            Console.Write("Initializing... ");
            if (File.Exists(path))
            {
                fs = new FSContainer(path);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("FS not found. Creating new FS.");
                int clusterSize = 0;
                while (clusterSize == 0)
                {
                    Console.Write("Enter cluster size (bytes): ");
                    string? input = Console.ReadLine();
                    if (input == null || !int.TryParse(input, out clusterSize))
                    {
                        Console.Write("Invalid value. ");
                    }
                }
                long totalSize = 0;
                while (totalSize == 0)
                {
                    Console.Write("Enter total size (bytes): ");
                    string? input = Console.ReadLine();
                    if (input == null || !long.TryParse(input, out totalSize))
                    {
                        Console.Write("Invalid value. ");
                    }
                }

                fs = new FSContainer(path, clusterSize, totalSize);
            }

            Console.WriteLine($"Done. Cluster size: {fs.BlockSize} bytes; Total size: {fs.ContainerSize} bytes.");
            while (!exiting)
            {
                Console.Write('>');
                string? input = Console.ReadLine();
                ProcessInput(input);
            }
        }

        private static void ProcessInput(string? input)
        {
            if (input == null || input.Length == 0)
            {
                return;
            }

            string[] split = input.FSSplit(' ', 2);

            string command = split[0];
            string param = split[1];

            try
            {
                switch (command)
                {
                    case "mkdir": //Създаване на директория 
                        {
                            fs.CreateDirectory(param);
                            break;
                        }
                    case "rmdir": //Изтриване на празна директория 
                        {
                            fs.DeleteDirectory(param);
                            break;
                        }
                    case "ls": //Извеждане на съдържанието на директория 
                        {
                            break;
                        }
                    case "cd": //Промяна на текущата директория 
                        {
                            if (fs.DirectoryExists(param))
                            {
                                //Directory.SetCurrentDirectory(param);
                            }
                            break;
                        }
                    case "cp": //Копиране на файл 
                        {
                            ///
                            break;
                        }
                    case "rm": //Изтриване на файл 
                        {
                            fs.DeleteFile(param);
                            break;
                        }
                    case "cat": //Извеждане на съдържанието на файл на екрана 
                        {
                            fs.DisplayFileContent(param);
                            break;
                        }
                    case "write": //Записване на съдържание към файл 
                        {
                            break;
                        }
                    case "import": //Копиране на файл от външна файлова 
                        {
                            break;
                        }
                    case "export": //Копиране на външна файлова система 
                        {
                            break;
                        }
                    case "exit": //Изход
                        {
                            exiting = true;
                            break;
                        }
                    default:
                        {
                            throw new InvalidOperationException();
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void DisplayFileContent(FSDirectory data, long key)
        {
            if (!data.IsDirectory)
            {
                foreach (var b in data.Data)
                {
                    Console.Write((char)b);
                }
            }
            else 
            {
                Console.WriteLine(data.FileAllocationTable[key]);
            }
        }

    }
}