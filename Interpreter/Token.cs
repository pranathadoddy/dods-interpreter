using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    enum TokenType
    {
        // Single-character tokens.
        LeftParen, RightParen, LeftBrace, RightBrace,
        Comma, Dot, Minus, Plus, Semicolon, Slash, Star,

        // One or two character tokens.
        Bang, BangEqual,
        Equal, EqualEqual,
        Greater, GreaterEqual,
        Less, LessEqual,

        // Literals.
        Identifier, String, Number,

        // Keywords.
        And, Class, Else, False, Fun, For, If, Nil, Or,
        Print, Return, Super, This, True, Var, While,

        Eof
    }

    internal class Token
    {
        private TokenType _type;
        private string _lexeme;
        private object? _literal;
        private int _line;

        public TokenType Type { get { return _type; } }
        public string Lexeme { get { return _lexeme; } }
        public object? Literal { get { return _literal; } }
        public int Line { get { return _line; } }   

        public Token(TokenType type, string lexeme, object? literal, int line)
        {
            this._type = type;
            this._lexeme = lexeme;
            this._literal = literal;
            this._line = line;
        }

        public string ToString()
        {
            return this._type + " " + this._lexeme + " "+ this._literal;
        }

    }
}
