namespace Interpreter
{
    internal class Parser
    {
        private readonly List<Token> _tokens;
        private int _current = 0;

        public Parser(List<Token> tokens) 
        { 
            this._tokens = tokens;
        }


        private bool Match(params TokenType[] types) {
            foreach (var type in types) {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;  
        }

        private bool Check(TokenType type)
        {
            if(IsAtEnd()) return false;
            return Peek().Type == type;
        }

        private Token Advance() {
            if (!IsAtEnd()) _current++;
            return Previous();
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.Eof;
        }

        private Token Peek()
        {
            return _tokens[_current];
        }

        private Token Previous()
        {
            return _tokens[_current - 1];
        }
    }
}
