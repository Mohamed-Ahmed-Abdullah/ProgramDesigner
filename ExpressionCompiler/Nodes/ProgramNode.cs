using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using ExpressionCompiler.Common;
using Irony.Compiler;

namespace ExpressionCompiler.Nodes
{
    public class ProgramNode : AstNode, IExpressionGenerator
    {
        public ProgramNode(AstNodeArgs args): base(args)
        {}

        public static ExpressionVariableList VariableList { get; set; }

        public Expression GenerateExpression(object tree)
        {
            Expression lastExpressionNode = null;
            VariableList = new ExpressionVariableList();
            var block = new List<Expression>();
            lastExpressionNode = CompileStatementList(ChildNodes[0].ChildNodes);

            ////they all statement but, inside the statement held the tpe
            //foreach (var statement in ChildNodes[0].ChildNodes)
            //{
            //     if (statement.ChildNodes[0] is VariableDeclarationNode)
            //     {
            //         //var xx; // var is ChildNodes[0] , xx  is ChildNodes[1]
            //         var declaration = (VariableDeclarationNode)statement.ChildNodes[0];
            //         VariableList.Add((ParameterExpression)declaration.GenerateExpression(statement.ChildNodes[0].ChildNodes[1]));
            //    }
            //    else if (statement.ChildNodes[0] is VariableAssignmentNode)
            //    {
            //        //xx=2+3; // xx os a variable token , = is a symbol token , expression 
            //        var assignment = (VariableAssignmentNode)statement.ChildNodes[0];
            //        var expressionNode = (ExpressionNode) statement.ChildNodes[0].ChildNodes[2];
            //        var variableToken = (Token)statement.ChildNodes[0].ChildNodes[0];
            //        var expression1 = expressionNode.GenerateExpression(null);
            //        var expression2 = VariableList.Get(variableToken.Text);

            //        block.Add(assignment.GenerateExpression(new TwoExpressionsDto
            //            {
            //                Expression1 = expression1,
            //                Expression2 = expression2
            //            }));
            //    }
            //    else if (statement.ChildNodes[0] is ExpressionNode)
            //    {
            //        var expressionNode = (ExpressionNode) statement.ChildNodes[0];
            //        lastExpressionNode = expressionNode.GenerateExpression(null);
            //    }
            //     else if (statement.ChildNodes[0] is IfStatementNode)
            //     {
            //         var expressionNode = (IfStatementNode)statement.ChildNodes[0];
            //         block.Add(expressionNode.GenerateExpression(null));
            //     }
            //     else
            //     {
            //         Debug.WriteLine("Whaaaaaaaat how? it seems like you changed the grammer without changing the code");
            //     }
            //}


            if (lastExpressionNode != null)
            {
                block.Add(lastExpressionNode);
                lastExpressionNode = Expression.Block(block);
                var lambda = Expression.Lambda<Func<decimal?>>(Expression.Block(
                    VariableList.Select(q => (ParameterExpression) q.FirstExpression).ToArray(),
                    lastExpressionNode));
                return lambda;
            }
            throw new Exception("there is no equations (expressions) in your code");
        }

        public static Expression CompileStatementList(AstNodeList childNode)
        {
            Expression lastExpressionNode = null;
            var block = new List<Expression>();

            //they all statement but, inside the statement held the tpe
            foreach (var statement in childNode)
            {
                 if (statement.ChildNodes[0] is VariableDeclarationNode)
                 {
                     //var xx; // var is ChildNodes[0] , xx  is ChildNodes[1]
                     var declaration = (VariableDeclarationNode)statement.ChildNodes[0];
                     VariableList.Add((ParameterExpression)declaration.GenerateExpression(statement.ChildNodes[0].ChildNodes[1]));
                }
                else if (statement.ChildNodes[0] is VariableAssignmentNode)
                {
                    //xx=2+3; // xx os a variable token , = is a symbol token , expression 
                    var assignment = (VariableAssignmentNode)statement.ChildNodes[0];
                    var expressionNode = (ExpressionNode) statement.ChildNodes[0].ChildNodes[2];
                    var variableToken = (Token)statement.ChildNodes[0].ChildNodes[0];
                    var expression1 = expressionNode.GenerateExpression(null);
                    var expression2 = VariableList.Get(variableToken.Text);

                    block.Add(assignment.GenerateExpression(new TwoExpressionsDto
                        {
                            Expression1 = expression1,
                            Expression2 = expression2
                        }));
                }
                else if (statement.ChildNodes[0] is ExpressionNode)
                {
                    var expressionNode = (ExpressionNode) statement.ChildNodes[0];
                    lastExpressionNode = expressionNode.GenerateExpression(null);
                }
                 else if (statement.ChildNodes[0] is IfStatementNode)
                 {
                     var expressionNode = (IfStatementNode)statement.ChildNodes[0];
                     block.Add(expressionNode.GenerateExpression(null));
                 }
                 else
                 {
                     Debug.WriteLine("Whaaaaaaaat how? it seems like you changed the grammer without changing the code");
                 }
            }

            if (lastExpressionNode != null)
            {
                block.Add(lastExpressionNode);
                return Expression.Block(block);
            }

            if (block.Any())
            {
                return Expression.Block(block);
            }
            return null;
        }
    }
}