using System.Text;

namespace Interpreter
{
    internal class AstPrinter : Expr.IVisitor<string>
    {
        public AstPrinter() { }

        public string Print(Expr expr)
        {
            return expr.Accept(this);
        }

        private string Parenthesize(string name, params Expr[] exprs)
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

        string Expr.IVisitor<string>.VisitBinaryExpr(Expr.Binary expr)
        {
            return Parenthesize(expr.Operator.Lexeme, expr.Left, expr.Right);
        }

        string Expr.IVisitor<string>.VisitLiteralExpr(Interpreter.Expr.Literal expr)
        {
            if (expr.Value == null) return "nil";

            return expr.Value.ToString(); 
        }

        string Expr.IVisitor<string>.VisitVariableExpr(Interpreter.Expr.Variable expr)
        {
            return "";
        }

        string Expr.IVisitor<string>.VisitGroupingExpr(Interpreter.Expr.Grouping expr)
        {
            return Parenthesize("group", expr.Expression);
        }

        string Expr.IVisitor<string>.VisitUnaryExpr(Interpreter.Expr.Unary expr)
        {
            return Parenthesize(expr.Operator.Lexeme, expr.Right);
        }
    }
}
