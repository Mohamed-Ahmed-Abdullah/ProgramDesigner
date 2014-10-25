using System.Linq.Expressions;
using ExpressionCompiler.Common;
using Irony.Compiler;

namespace ExpressionCompiler.Nodes
{
    public class ElseStatementNode : AstNode , IExpressionGenerator
    {
        public ElseStatementNode(AstNodeArgs args) : base(args)
        {}

        public Expression GenerateExpression(object tree)
        {
            return null;
        }
    }
}