namespace Interpreter
{
    internal class DataStore
    {
        private Dictionary<string, object> _values = new Dictionary<string, object>();

        public void Define(string name, object value)
        {
            _values[name] = value;
        }

        public Object GetValue(Token token) { 
            if(_values.ContainsKey(token.Lexeme)) return _values[token.Lexeme];

            throw new RuntimeError(token,$"Undefined variable {token.Lexeme}");
        }
    }
}
