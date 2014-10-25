using System.Linq.Expressions;

namespace ExpressionCompiler.Common
{
    //if you understand how the parameter expression works you will get this easly, Expression.Assign returns BinaryExpression you need this to complete the equation but you need the first ParameterExpression to pass it to the lambda
    public class ExpressionVariable
    {
        public string Key { get; set; }

        private Expression _variableExpression;
        public Expression VariableExpression
        {
            get { return _variableExpression; }
            set
            {
                if (FirstExpression == null)
                    FirstExpression = value;
                _variableExpression = value;
            }
        }

        public Expression AssigmentExpression { get; set; }
        public Expression FirstExpression { get; set; }
    }
}