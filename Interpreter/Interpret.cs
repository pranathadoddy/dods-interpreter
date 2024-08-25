namespace Interpreter
{
    internal class Interpret : Expr.IVisitor<object>, Stmt.IVisitor<object>
    {

        public void Parse(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt stmt in statements) { 
                    Execute(stmt);
                }
            }
            catch (RuntimeError error)
            {
                Program.RuntimeError(error);
            }

        }

        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }

        object Expr.IVisitor<object>.VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.Value;
        }

        object Expr.IVisitor<object>.VisitGroupingExpr(Interpreter.Expr.Grouping expr)
        {
            return Evaluate(expr.Expression);
        }

        object Expr.IVisitor<object>.VisitUnaryExpr(Expr.Unary expr)
        {
            var right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.Bang:
                    return !IsTruthy(right);
                case TokenType.Minus:
                    return -(double)right;
            }

            return null;
        }

        object Expr.IVisitor<object>.VisitBinaryExpr(Interpreter.Expr.Binary expr)
        {
            var right = Evaluate(expr.Right);
            var left = Evaluate(expr.Left);

            switch (expr.Operator.Type)
            {
                case TokenType.Plus:
                    if (left is double && right is double)
                    {
                        return (double)left + (double)right;
                    }

                    if (left is string && right is string)
                    {
                        return (string)left + (string)right;
                    }

                    throw new RuntimeError(expr.Operator, "Operand must be two numbers or two strings.");

                case TokenType.Minus:
                    CheckNumberOperand(expr.Operator, right);
                    return (double)left - (double)right;
                case TokenType.Slash:
                    CheckNumberOperand(expr.Operator, right);
                    return (double)left / (double)right;
                case TokenType.Star:
                    CheckNumberOperand(expr.Operator, right);
                    return (double)left * (double)right;
                case TokenType.Greater:
                    CheckNumberOperand(expr.Operator, right);
                    return (double)left > (double)right;
                case TokenType.GreaterEqual:
                    CheckNumberOperand(expr.Operator, right);
                    return (double)left >= (double)right;
                case TokenType.Less:
                    CheckNumberOperand(expr.Operator, right);
                    return (double)left < (double)right;
                case TokenType.LessEqual:
                    CheckNumberOperand(expr.Operator, right);
                    return (double)left <= (double)right;
                case TokenType.EqualEqual:
                    return IsEqual(left, right);
                case TokenType.BangEqual:
                    return !IsEqual(left, right);
            }

            return null;
        }

        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            return true;
        }

        private bool IsEqual(object obj1, object obj2)
        {
            if (obj1 == null && obj2 == null) return true;
            if (obj1 == null) return false;

            return obj1.Equals(obj2);
        }


        private object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        private void CheckNumberOperand(Token @operator, object operand)
        {
            if (operand is double) return;
            throw new RuntimeError(@operator, "Operand must be a number.");
        }

        private string Stringify(object obj)
        {
            if (obj == null) return "nil";

            if (obj is double)
            {
                string text = obj.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
                return text;
            }

            return obj.ToString();
        }

        object Stmt.IVisitor<object>.VisitExpressionStatement(Interpreter.Stmt.Expression stmt) 
        {
            Evaluate(stmt.Expr);
            return null;
        }

        object Stmt.IVisitor<object>.VisitPrintStatement(Interpreter.Stmt.Print stmt)
        {
            var value = Evaluate(stmt.Expr);
            Console.WriteLine(Stringify(value));
            return null;
        }
    }
}
