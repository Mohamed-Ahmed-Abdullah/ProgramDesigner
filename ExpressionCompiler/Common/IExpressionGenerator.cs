using System.Linq.Expressions;

namespace ExpressionCompiler.Common
{
    public interface IExpressionGenerator
    {
        Expression GenerateExpression(object tree);
    }
}