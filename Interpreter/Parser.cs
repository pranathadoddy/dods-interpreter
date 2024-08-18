namespace Interpreter
{
    internal class Parser
    {
        private readonly List<Token> _tokens;
        private int _current = 0;

        private  class ParseError : Exception
        {

        } 

        public Parser(List<Token> tokens) 
        { 
            this._tokens = tokens;
        }

        public Expression Parse()
        {
            try
            {
                return Expression();
            }
            catch (ParseError error)
            {
                Synchronize();
                return null;
            }
        }

        private Expression Expression() {
            return Equality();
        }

        private Expression Equality()
        {
            Expression expr = Comparison();

            while (Match(TokenType.BangEqual, TokenType.EqualEqual))
            {
                Token @operator = Previous();
                Expression right = Comparison();
                expr = new Expression.Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expression Comparison()
        {
            Expression expr = Term();

            while (Match(TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual))
            {
                Token @operator = Previous();
                Expression right = Term();
                expr = new Expression.Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expression Term() {
            Expression expr = Factor();

            while (Match(TokenType.Minus, TokenType.Plus))
            {
                Token @operator = Previous();
                Expression right = Factor();
                expr = new Expression.Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expression Factor() {
            Expression expr = Unary();

            while (Match(TokenType.Slash, TokenType.Star))
            {
                Token @operator = Previous();
                Expression right = Unary();
                expr = new Expression.Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expression Unary() {
            if (Match(TokenType.Bang, TokenType.Minus))
            {
                Token @operator = Previous();
                Expression right = Unary();
                return new Expression.Unary(@operator, right);
            }

            return Primary();
        }

        private Expression Primary() {
            if(Match(TokenType.False)) return new Expression.Literal(false);
            if(Match(TokenType.True)) return new Expression.Literal(true);
            if(Match(TokenType.Nil)) return new Expression.Literal(null);

            if(Match(TokenType.Number, TokenType.String))
            {
                return new Expression.Literal(Previous().Literal);
            }

            if (Match(TokenType.LeftParen))
            {
                Expression expression = Expression();
                Consume(TokenType.RightParen, "Expect ')' after expression.");
                return new Expression.Grouping(expression);
            }

            throw Error(Peek(), "Expect expression.");
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

        private Token Consume(TokenType type, string message) { 
            if(Check(type)) return Advance();

            throw Error(Peek(),message);
        }

        private ParseError Error(Token token, string message) {
            Program.Error(token, message);
            return new ParseError();
        }

        private void Synchronize()
        {
            Advance();

            while (!IsAtEnd())
            {
                if (Previous().Type == TokenType.Semicolon) return;

                switch (Peek().Type)
                {
                    case TokenType.Class:
                    case TokenType.Fun:
                    case TokenType.Var:
                    case TokenType.For:
                    case TokenType.If:
                    case TokenType.While:
                    case TokenType.Print:
                    case TokenType.Return:
                        return;
                }

                Advance();
            }
        }
    }
}
