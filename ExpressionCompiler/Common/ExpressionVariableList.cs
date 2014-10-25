using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionCompiler.Common
{
    public class ExpressionVariableList : List<ExpressionVariable>
    {
        public new void Add(ParameterExpression expressionVariable)
        {
            var variable = this.FirstOrDefault(q => q.Key == expressionVariable.Name);
            if(variable != null)
                throw new Exception("there is a variable has the same name " + expressionVariable.Name);
            Add(new ExpressionVariable {Key = expressionVariable.Name, VariableExpression = expressionVariable});
        }

        public Expression Get(string key)
        {
            var variable = this.FirstOrDefault(q => q.Key == key);
            if (variable == null)
                throw new Exception("there is no variable named " + key);
            return variable.VariableExpression;
        }

        public void UpdateExpression(string key, Expression newExpression)
        {
            var variable = this.FirstOrDefault(q => q.Key == key);
            if (variable == null)
                throw new Exception("there is no variable named " + key);
            variable.VariableExpression = newExpression;
        }
    }
}