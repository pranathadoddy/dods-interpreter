using System.IO;
using System.Text;

namespace Interpreter
{
    internal class Program
    {
        private static bool hadError = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }

        private static void RunFile(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            string content = Encoding.Default.GetString(bytes);

            Run(content);
        }

        private static void RunPrompt()
        {
            using (var reader = new StreamReader(Console.OpenStandardInput()))
            {
                while (true)
                {
                    Console.Write("> ");

                    string line = reader.ReadLine();
                    if (line == null) break;
                    Run(line);
                    hadError = true;

                }
            }
        }

        private static void Run(string source)
        {

        }

        private static void Error(int line, string message) { 
            Report(line, "", message);  
        }

        private static void Report(int line, string where, string message) { 
            Console.WriteLine($"[line {line}] Error {where} : {message}");
            hadError = true;
        }
    }
}
