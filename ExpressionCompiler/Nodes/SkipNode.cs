using System.Linq.Expressions;
using ExpressionCompiler.Common;
using Irony.Compiler;

namespace ExpressionCompiler.Nodes
{
    public class SkipNode : AstNode , IExpressionGenerator
    {
        public SkipNode(AstNodeArgs args) : base(args)
        {
        }

        public Expression GenerateExpression(object tree)
        {
            var _2 = Expression.Constant((decimal?)2, typeof(decimal?));

            var add = Expression.Add(_2, _2);

            return add;
        }
    }
}