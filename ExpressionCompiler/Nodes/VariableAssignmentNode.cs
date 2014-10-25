using System.Linq.Expressions;
using ExpressionCompiler.Common;
using Irony.Compiler;

namespace ExpressionCompiler.Nodes
{
    public class VariableAssignmentNode : AstNode, IExpressionGenerator
    {
        public VariableAssignmentNode(AstNodeArgs args): base(args)
        {}

        public Expression GenerateExpression(object tree)
        {
            var twoExpressionsDto = (TwoExpressionsDto)tree;
            return Expression.Assign(twoExpressionsDto.Expression2, twoExpressionsDto.Expression1);
        }
    }
}