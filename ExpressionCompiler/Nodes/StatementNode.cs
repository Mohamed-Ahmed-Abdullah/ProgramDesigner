using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ExpressionCompiler.Common;
using Irony.Compiler;

namespace ExpressionCompiler.Nodes
{
    public class StatementNode : AstNode, IExpressionGenerator
    {
        public StatementNode(AstNodeArgs args) : base(args)
        {}

        public Expression GenerateExpression(object tree)
        {
            return ProgramNode.CompileStatementList(ChildNodes);
        }
    }
}