using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionCompiler.Common;
using Irony.Compiler;

namespace ExpressionCompiler.Nodes
{
    public class VariableDeclarationNode : AstNode, IExpressionGenerator
    {
        public VariableDeclarationNode(AstNodeArgs args): base(args)
        {}

        public Expression GenerateExpression(object tree)
        {
            var variable = (Token) tree;
            return Expression.Parameter(typeof(decimal?), variable.Text);
        }
    }
}
