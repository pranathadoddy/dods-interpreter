using static Interpreter.Expression;
using System.Collections.Generic;
using System.Linq;

namespace Interpreter
{
    internal abstract class Expression
    {
        internal interface IVisitor<R>
        {
            R VisitBinaryExpr(Binary expr);
            R VisitLiteralExpr(Literal expr);
            R VisitGroupingExpr(Grouping expr);
            R VisitUnaryExpr(Unary expr);

        }

        internal abstract R Accept<R>(IVisitor<R> visitor);

        public class Binary : Expression
        {
            public Binary(Expression left, Token @operator, Expression right)
            {
                Left = left;
                Right = right;  
                Operator = @operator;
            }

            public  Expression Left;
            public  Token Operator;
            public  Expression Right;

            internal override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitBinaryExpr(this);
            }
        }

        public class Literal : Expression
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

        public class Grouping : Expression
        {
            public Grouping(Expression expression)
            {
                Expression = expression;
            }

            internal override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitGroupingExpr(this);
            }

            public readonly Expression Expression;
        }

        public class Unary : Expression
        {
            public Unary(Token @operator, Expression right)
            {
                Operator = @operator;
                Right = right;
            }

            internal override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitUnaryExpr(this);
            }

            public readonly Token Operator;
            public readonly Expression Right;
        }
    }

}
