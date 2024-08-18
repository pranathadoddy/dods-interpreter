using System.Text;

namespace Interpreter
{
    internal class AstPrinter : Expression.IVisitor<string>
    {
        public AstPrinter() { }

        public string Print(Expression expr)
        {
            return expr.Accept(this);
        }

        private string Parenthesize(string name, params Expression[] exprs)
        {
            var builder = new StringBuilder();

            builder.Append("(").Append(name);
            foreach (var expr in exprs)
            {
                builder.Append(" ");
                builder.Append(expr.Accept(this));
            }
            builder.Append(")");

            return builder.ToString();
        }

        string Expression.IVisitor<string>.VisitBinaryExpr(Expression.Binary expr)
        {
            return Parenthesize(expr.Operator.Lexeme, expr.Left, expr.Right);
        }

        string Expression.IVisitor<string>.VisitLiteralExpr(Interpreter.Expression.Literal expr)
        {
            if (expr.Value == null) return "nil";

            return expr.Value.ToString(); 
        }

        string Expression.IVisitor<string>.VisitGroupingExpr(Interpreter.Expression.Grouping expr)
        {
            return Parenthesize("group", expr.Expression);
        }

        string Expression.IVisitor<string>.VisitUnaryExpr(Interpreter.Expression.Unary expr)
        {
            return Parenthesize(expr.Operator.Lexeme, expr.Right);
        }
    }
}
