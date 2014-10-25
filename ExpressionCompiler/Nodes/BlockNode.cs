using System.Linq.Expressions;
using ExpressionCompiler.Common;
using Irony.Compiler;
using System.Diagnostics;

namespace ExpressionCompiler.Nodes
{
    public class BlockNode: AstNode , IExpressionGenerator
    {
        public BlockNode(AstNodeArgs args) : base(args)
        {}

        public Expression GenerateExpression(object tree)
        {
            //var _2 = Expression.Constant((decimal?)2, typeof(decimal?));
            //var _3 = Expression.Constant((decimal?)3, typeof(decimal?));
            //var _4 = Expression.Constant((decimal?)4, typeof(decimal?));

            //var conditionResult = Expression.Condition(Expression.Equal(_2, _2), _3, _4);

            //var add = Expression.Add(_2, tree);
            //var mult = Expression.Multiply(add, _4);

            //return add;

            //list of variableDeclarations and Expressions and variablesAsssinments
             //((ExpressionNode)ChildNodes[0]).GenerateExpression(null);
            return null;
        }
    }
}