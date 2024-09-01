using System.Text;

namespace Interpreter
{
    internal class Program
    {
        private static bool hadError = false;
        private static bool hadRuntimeError = false;
        private static readonly Interpret _interpreter = new Interpret();

        static void Main(string[] args)
        {
            if (args.Length > 2)
            {
                Console.WriteLine("Usage: dod <filename>");
                return;
            }

            if(args.Length == 2)
            {
                string command = args[0];
                string path = args[1];

                if (command.ToLower() == "dod")
                {
                    RunFile(path);
                    return;
                }
            }

            RunPrompt();
        }

        private static void RunFile(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            string content = Encoding.Default.GetString(bytes);

            Run(content);

            if (hadError) Environment.Exit(65);

            if (hadRuntimeError) Environment.Exit(70);
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
            var statements = parser.Parse();

            
            if (hadError) return;

            _interpreter.Parse(statements);
        }

        public static void Error(int line, string message) { 
            Report(line, "", message);  
        }

        public static void Error(Token token, string message)
        {
            if (token.Type == TokenType.Eof)
            {
                Report(token.Line, " pada akhir", message);
            }
            else
            {
                Report(token.Line, $" pada '{token.Lexeme}'", message);
            }
        }

        private static void Report(int line, string where, string message) { 
            Console.WriteLine($"[Baris {line}] Error {where} : {message}");
            hadError = true;
        }

        public static void RuntimeError(RuntimeError error) {
            Console.Error.WriteLine(error.Message +
        $"\n[Baris {error.Token.Line}]");
            hadRuntimeError = true;
        }
    }
}
