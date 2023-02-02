using FSLibrary;
using FSTools;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace FSConsole
{
    internal class Program
    {
        private static bool exiting = false;
        private static void Main(string[] args)
        {
            string path = Console.ReadLine();

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
                            FS.CreateDirectory(param);
                            break;
                        }
                    case "rmdir": //Изтриване на празна директория 
                        {
                            FS.DeleteDirectory(param);
                            break;
                        }
                    case "ls": //Извеждане на съдържанието на директория 
                        {
                            FS.GetSubdirectoriesAndFiles(param);
                            break;
                        }
                    case "cd": //Промяна на текущата директория 
                        {
                            if (FS.DirectoryExists(param))
                            {
                                Directory.SetCurrentDirectory(param);
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
                            FS.DeleteFile(param);
                            break;
                        }
                    case "cat": //Извеждане на съдържанието на файл на екрана 
                        {
                            FS.DisplayFileContent(param);
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

        private static void FormatGetSubdirectoriesAndFilesOutput(FSDictionary<string, string[]> dirsAndFiles, string path, string[] keyDirs)
        {
            foreach (string key in keyDirs)
            {
                foreach (var fileName in dirsAndFiles[key])
                {
                    FSString.FSSplit(fileName, '\\', StringSplitOptions.None);
                }
            }
        }

    }
}