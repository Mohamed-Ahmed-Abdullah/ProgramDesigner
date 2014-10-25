using System;
using System.Linq.Expressions;
using ExpressionCompiler.Common;
using Irony.Compiler;

namespace ExpressionCompiler.Nodes
{
    public class ExpressionNode : AstNode, IExpressionGenerator
    {
        public ExpressionNode(AstNodeArgs args) : base(args)
        {}

        public Expression GenerateExpression(object tree)
        {
            return GetExpressionFrom(ChildNodes);
        }

        private Expression GetExpressionFrom(AstNodeList astNodeList)
        {
            if (astNodeList.Count == 1)
            {
                return GetExpression((Token) astNodeList[0]);
            }

            Expression firstExpression, secondExpression;

            var operationToken = (Token) astNodeList[1].ChildNodes[0];

            if (astNodeList[0].ChildNodes != null && astNodeList[0].ChildNodes.Count != 1)
            {
                firstExpression = GetExpressionFrom(astNodeList[0].ChildNodes);
            }
            else
            {
                firstExpression = GetExpression(((Token) astNodeList[0].ChildNodes[0]));
                //firstExpression = Expression.Constant(decimal.Parse(((Token) astNodeList[0].ChildNodes[0]).Text),
                //                                      typeof (decimal?));
            }


            if (astNodeList[2].ChildNodes != null && astNodeList[2].ChildNodes.Count != 1)
            {
                secondExpression = GetExpressionFrom(astNodeList[2].ChildNodes);
            }
            else
            {
                secondExpression = GetExpression(((Token)astNodeList[2].ChildNodes[0]));
                //secondExpression = Expression.Constant(decimal.Parse(((Token) astNodeList[2].ChildNodes[0]).Text),
                //                                       typeof (decimal?));
            }
            return DoOperation(operationToken, firstExpression, secondExpression);
        }

        private Expression DoOperation(Token operation,Expression firstExpression,Expression secondExpression)
        {
            switch (operation.Text.Trim())
            {
                case "/":
                    return Expression.Divide(firstExpression, secondExpression);
                case "+":
                    return Expression.Add(firstExpression, secondExpression);
                case "-":
                    return Expression.Subtract(firstExpression, secondExpression);
                case "*":
                    return Expression.Multiply(firstExpression, secondExpression);
                case ">":
                    return Expression.GreaterThan(firstExpression, secondExpression);
                case "<":
                    return Expression.LessThan(firstExpression, secondExpression);
                case "==":
                    return Expression.Equal(firstExpression, secondExpression);
                case "!=":
                    return Expression.NotEqual(firstExpression, secondExpression);
                case "<=":
                    return Expression.LessThanOrEqual(firstExpression, secondExpression);
                case ">=":
                    return Expression.GreaterThanOrEqual(firstExpression, secondExpression);

                default: 
                    return Expression.Constant(2, typeof(decimal?));
            }
        }

        private Expression GetExpression(Token token)
        {
            if (token.Term.Name == "variable")
            {
                return GetTheVariablesList(ChildNodes[0]).Get(token.Text);
            }
            if (token.Term.Name == "number")
            {
                return Expression.Constant(decimal.Parse(token.Text), typeof(decimal?));
            }
            throw new Exception("something went wrong in the ExceptionNode.GetExpression()");
        }

        private ExpressionVariableList GetTheVariablesList(AstNode astNodeList)
        {
            return ProgramNode.VariableList;
            //if (astNodeList.Parent != null)
            //{
            //    return GetTheVariablesList(astNodeList.Parent);
            //}
            //return ((ProgramNode)astNodeList).VariableList;
        }
    }
}