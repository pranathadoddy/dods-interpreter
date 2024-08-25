using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    internal class RuntimeError : Exception
    {
        public Token Token { get; }

        public RuntimeError(Token token, string message) : base(message)
        {
            Token = token;
        }
    }
}
