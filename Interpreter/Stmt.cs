using static Interpreter.Expr;

namespace Interpreter
{
    internal abstract class Stmt
    {
        internal interface IVisitor<R>
        {
            R VisitExpressionStatement(Expression stmt);
            R VisitPrintStatement(Print stmt);

            R VisitVarStatement(Var var);
        }

        internal abstract R Accept<R>(IVisitor<R> visitor);

        public class Expression : Stmt
        {
            public Expression(Expr expr) { 
                this.Expr = expr;
            }

            public Expr Expr;

            internal override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitExpressionStatement(this);
            }
        }

        public class Print : Stmt
        {
            public Print(Expr expr)
            {
                this.Expr = expr;
            }

            public Expr Expr;

            internal override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitPrintStatement(this);
            }
        }

        public class Var : Stmt
        {
            public Var(Token name, Expr expr)
            {
                this.Expr = expr;
                this.Name = name;
            }

            public Expr Expr;
            public Token Name;

            internal override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitVarStatement(this);
            }
        }
    }
}
