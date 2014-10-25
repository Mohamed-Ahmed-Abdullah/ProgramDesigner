using System.Linq.Expressions;
using ExpressionCompiler.Common;
using Irony.Compiler;

namespace ExpressionCompiler.Nodes
{
    public class IfStatementNode : AstNode , IExpressionGenerator
    {
        public IfStatementNode(AstNodeArgs args) : base(args)
        {}

        //if Expression {statementList} else {stetmentList}
        //or
        //if Expression {statemnetList}
        public Expression GenerateExpression(object tree)
        {
            var comparison = ((ExpressionNode)ChildNodes[1]).GenerateExpression(null);
            //if
            var conditionTrueExpression = ((StatementNode)ChildNodes[3]).GenerateExpression(null);
            //else
            Expression conditionFalseExpression = null;
            if (ChildNodes != null && ChildNodes.Count >= 5 && ChildNodes[5].ChildNodes.Count >= 2)
            {
                conditionFalseExpression = ((StatementNode) ChildNodes[5].ChildNodes[2]).GenerateExpression(null);
            }

            return Expression.Condition(comparison,
                conditionTrueExpression ?? Expression.Constant(null, typeof(decimal?)),
                conditionFalseExpression ?? Expression.Constant(null, typeof(decimal?)));
        }
    }
}