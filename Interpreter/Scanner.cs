namespace Interpreter
{
    internal class Scanner
    {
        private string _source;
        private List<Token> _tokens = new List<Token>();
        private int _start = 0;
        private int _current = 0;
        private int _line = 1;
        private static Dictionary<string, TokenType> _keywords;

        public Scanner(string source)
        {
            this._source = source;

            _keywords = new Dictionary<string, TokenType>
            {
                { "and", TokenType.And },
                { "class", TokenType.Class },
                { "else", TokenType.Else },
                { "false", TokenType.False },
                { "for", TokenType.For },
                { "fun", TokenType.Fun },
                { "if", TokenType.If },
                { "nil", TokenType.Nil },
                { "or", TokenType.Or },
                { "print", TokenType.Print },
                { "return", TokenType.Return },
                { "super", TokenType.Super },
                { "this", TokenType.This },
                { "true", TokenType.True },
                { "var", TokenType.Var },
                { "while", TokenType.While }
            };
        }

        public List<Token> GenerateTokenList()
        {
            while (!IsAtEnd())
            {
                _start = _current;
                ScanToken();
            }

            this._tokens.Add(new Token(TokenType.Eof, "", null, _line));
            return this._tokens;
        }

        private void ScanToken()
        {
            var c = Advance();

            switch (c)
            {
                case '(': AddToken(TokenType.LeftParen); break;
                case ')': AddToken(TokenType.RightParen); break;
                case '{': AddToken(TokenType.LeftBrace); break;
                case '}': AddToken(TokenType.RightBrace); break;
                case ',': AddToken(TokenType.Comma); break;
                case '.': AddToken(TokenType.Dot); break;
                case '-': AddToken(TokenType.Minus); break;
                case '+': AddToken(TokenType.Plus); break;
                case ';': AddToken(TokenType.Semicolon); break;
                case '*': AddToken(TokenType.Star); break;
                case '!': AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang); break;
                case '=': AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal); break;
                case '>': AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater); break;
                case '<': AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less); break;
                case '/':
                    if (Match('/'))
                    {
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else
                    {
                        AddToken(TokenType.Slash);
                    }
                    break;

                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace.
                    break;

                case '\n':
                    _line++;
                    break;
                case '"':
                    HandleString(); break;

                default:
                    if (IsDigit(c))
                    {
                        HandleNumber();
                        break;
                    }

                    if (IsAlpha(c))
                    {
                        HandleIdentifier();
                        break;  
                    }

                    Program.Error(_line, "Unexpected character.");
                    break;
            }
        }


        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (_source[_current] != expected) return false;

            _current++;
            return true;
        }

        private char Advance()
        {
            return _source[_current++];

        }


        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }


        private void AddToken(TokenType type, object? literal)

        {
            var text = _source.Substring(_start, _current - _start);
            _tokens.Add(new Token(type, text, literal, _line));
        }

        private bool IsAtEnd()
        {
            return _current >= _source.Length;
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[_current];
        }

        private void HandleString()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() != '\n') _line++;

                Advance();
            }

            if (IsAtEnd())
            {
                Program.Error(_line, "Unexpected string");
                return;
            }

            Advance();

            var length = _current - _start;

            var value = _source.Substring(_start + 1, length - 2);
            AddToken(TokenType.String, value);
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private char PeekNext()
        {
            if (_current + 1 >= _source.Length) return '\0';
            return _source[_current + 1];
        }

        private void HandleNumber()
        {
            while (IsDigit(Peek()))
            {
                Advance();
            }

            if (Peek() == '.' && IsDigit(Peek()))
            {
                Advance();

                while (IsDigit(Peek()))
                {
                    Advance();
                }
            }

            var length = _current - _start;

            AddToken(TokenType.Number, Double.Parse(_source.Substring(_start, length)));
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                    c == '_';
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private void HandleIdentifier()
        {
            while (IsAlphaNumeric(Peek()))
            {
                Advance();
            }

            var length = _current - _start;
            var text = _source.Substring(_start, length);

            TokenType type = !_keywords.ContainsKey(text) ? TokenType.Identifier : _keywords[text];
            

            AddToken(TokenType.Identifier);
        }

    }
}
