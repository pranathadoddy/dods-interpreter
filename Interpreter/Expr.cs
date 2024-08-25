namespace Interpreter
{
    internal abstract class Expr
    {
        internal interface IVisitor<R>
        {
            R VisitBinaryExpr(Binary expr);
            R VisitLiteralExpr(Literal expr);
            R VisitGroupingExpr(Grouping expr);
            R VisitUnaryExpr(Unary expr);

        }

        internal abstract R Accept<R>(IVisitor<R> visitor);

        public class Binary : Expr
        {
            public Binary(Expr left, Token @operator, Expr right)
            {
                Left = left;
                Right = right;  
                Operator = @operator;
            }

            public  Expr Left;
            public  Token Operator;
            public  Expr Right;

            internal override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitBinaryExpr(this);
            }
        }

        public class Literal : Expr
        {
            public Literal(object value)
            {
                Value = value;
            }

            internal override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitLiteralExpr(this);
            }

            public readonly object Value;
        }

        public class Grouping : Expr
        {
            public Grouping(Expr expression)
            {
                Expression = expression;
            }

            internal override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitGroupingExpr(this);
            }

            public readonly Expr Expression;
        }

        public class Unary : Expr
        {
            public Unary(Token @operator, Expr right)
            {
                Operator = @operator;
                Right = right;
            }

            internal override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitUnaryExpr(this);
            }

            public readonly Token Operator;
            public readonly Expr Right;
        }
    }

}
