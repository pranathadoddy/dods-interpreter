using System.Text;

namespace Interpreter
{
    internal class Program
    {
        private static bool hadError = false;

        static void Main(string[] args)
        {
            RunPrompt();
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
                    hadError = false;

                }
            }
        }

        private static void Run(string source)
        {
            var scanner = new Scanner(source);
            var tokens = scanner.GenerateTokenList();

            //foreach (var token in tokens) { 
            //    Console.WriteLine(token);
            //}

            var parser = new Parser(tokens);
            var expression = parser.Parse();

            if (hadError) return;

            var astPrinter = new AstPrinter();
            Console.WriteLine(astPrinter.Print(expression));
        }

        public static void Error(int line, string message) { 
            Report(line, "", message);  
        }

        public static void Error(Token token, string message)
        {
            if (token.Type == TokenType.Eof)
            {
                Report(token.Line, " at end", message);
            }
            else
            {
                Report(token.Line, $" at '{token.Lexeme}'", message);
            }
        }

        private static void Report(int line, string where, string message) { 
            Console.WriteLine($"[line {line}] Error {where} : {message}");
            hadError = true;
        }
    }
}
