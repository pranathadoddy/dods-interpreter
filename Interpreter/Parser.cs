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

        public List<Stmt> Parse()
        {
            try
            {
                var statements = new List<Stmt>();

                while (!IsAtEnd()) {
                    statements.Add(Declaration());
                }

                return statements;
            }
            catch (ParseError error)
            {
                Synchronize();
                return null;
            }
        }

        private Stmt Declaration() {
            try
            {
                if (Match(TokenType.Var)) return VarDeclaration();

                return Statement();
            }
            catch (Exception exception)
            {
                Synchronize();
                return null;
            }
        }

        private Stmt Statement() { 
            if(Match(TokenType.Print)) return PrintStatement();

            return ExpressionStatement();
        }

        private Stmt PrintStatement() { 
            Expr value = Expression();
            Consume(TokenType.Semicolon, "Harap tambahkan titik koma (;) setelah ekspresi.");
            return new Stmt.Print(value);
        }

        public Stmt ExpressionStatement() {
            Expr value = Expression();
            Consume(TokenType.Semicolon, "Harap tambahkan titik koma (;) setelah ekspresi.");
            return new Stmt.Expression(value);
        }

        private Expr Expression() {
            return Equality();
        }

        private Expr Equality()
        {
            Expr expr = Comparison();

            while (Match(TokenType.BangEqual, TokenType.EqualEqual))
            {
                Token @operator = Previous();
                Expr right = Comparison();
                expr = new Expr.Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expr Comparison()
        {
            Expr expr = Term();

            while (Match(TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual))
            {
                Token @operator = Previous();
                Expr right = Term();
                expr = new Expr.Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expr Term() {
            Expr expr = Factor();

            while (Match(TokenType.Minus, TokenType.Plus))
            {
                Token @operator = Previous();
                Expr right = Factor();
                expr = new Expr.Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expr Factor() {
            Expr expr = Unary();

            while (Match(TokenType.Slash, TokenType.Star))
            {
                Token @operator = Previous();
                Expr right = Unary();
                expr = new Expr.Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expr Unary() {
            if (Match(TokenType.Bang, TokenType.Minus))
            {
                Token @operator = Previous();
                Expr right = Unary();
                return new Expr.Unary(@operator, right);
            }

            return Primary();
        }

        private Expr Primary() {
            if(Match(TokenType.False)) return new Expr.Literal(false);
            if(Match(TokenType.True)) return new Expr.Literal(true);
            if(Match(TokenType.Nil)) return new Expr.Literal(null);

            if(Match(TokenType.Number, TokenType.String))
            {
                return new Expr.Literal(Previous().Literal);
            }

            if (Match(TokenType.Identifier))
            {
                return new Expr.Variable(Previous());
            }

            if (Match(TokenType.LeftParen))
            {
                Expr expression = Expression();
                Consume(TokenType.RightParen, "Expect ')' after expression.");
                return new Expr.Grouping(expression);
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

        private Stmt VarDeclaration()
        {
            var name = Consume(TokenType.Identifier, "Expect variable name.");

            Expr initializer = null;
            if (Match(TokenType.Equal))
            {
                initializer = Expression();
            }

            Consume(TokenType.Semicolon, "Expect ';' after variable name");
            return new Stmt.Var(name, initializer);
        }
    }
}
