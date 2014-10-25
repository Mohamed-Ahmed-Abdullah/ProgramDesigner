using ExpressionCompiler.Nodes;
using Irony.Compiler;
using BlockNode = ExpressionCompiler.Nodes.BlockNode;
using SkipNode = ExpressionCompiler.Nodes.SkipNode;

namespace ExpressionCompiler
{
    public class FlGrammar : Grammar
    {
        public FlGrammar()
        {
            // turn off case sensitivity
            CaseSensitive = false;

            // define all the non-terminals
            var program = new NonTerminal("program", typeof(ProgramNode));
            var statementList = new NonTerminal("statementList", typeof(StatementNode));
            var statement = new NonTerminal("statement", typeof(SkipNode));
            var expression = new NonTerminal("expression", typeof(ExpressionNode));
            var binaryOperator = new NonTerminal("binaryOperator", typeof(SkipNode));

            var variableDeclaration = new NonTerminal("variableDeclaration", typeof(VariableDeclarationNode));
            var variableAssignment = new NonTerminal("variableAssignment", typeof(VariableAssignmentNode));

            var ifStatement = new NonTerminal("ifStatement", typeof(IfStatementNode));
            var elseStatement = new NonTerminal("elseStatement", typeof(ElseStatementNode));

            // define all the terminals
            var variable = new IdentifierTerminal("variable");
            variable.AddKeywords("set", "var" , "to", "if", "freight", "cost", "is", "loop", "through", "order");
            var number = new NumberLiteral("number");
            var stringLiteral = new StringLiteral("string", "\"", ScanFlags.None);

            RegisterPunctuation(";", "[", "]", "(", ")");
            //var lpar = new Terminal("(");
            //var rpar = new Terminal(")");
            //var lbr = new Terminal("{");
            //var rbr = new Terminal("}");


            // specify the non-terminal which is the root of the AST
            //Root = program;
            //binaryOperator.Rule = Symbol("+") | "-" | "*" | "/" | "<" | "==" | "!=" | ">" | "<=" | ">=" | "is";
            //program.Rule = statementList;
            //statementList.Rule = MakeStarRule(statementList, null, statement);
            //statement.Rule = variableDeclaration + ";" | variableAssignment + ";" | expression + ";" | ifStatement;
            //variableAssignment.Rule = variable + "=" + expression;
            //variableDeclaration.Rule = Symbol("var") + variable;
            //ifStatement.Rule = Symbol("if") + "(" + expression + ")"
            //    + "{" + expression + "}"
            //    + elseStatement;
            //elseStatement.Rule = Empty | "else" + "{" + expression + "}";
            //expression.Rule = number | variable | stringLiteral
            //    | expression + binaryOperator + expression
            //    | "(" + expression + ")";
            Root = program;
            binaryOperator.Rule = Symbol("+") | "-" | "*" | "/" | "<" | "==" | "!=" | ">" | "<=" | ">=" | "is";


            program.Rule = statementList;
            statementList.Rule = MakeStarRule(statementList, null, statement);
            statement.Rule = variableDeclaration + ";" | variableAssignment + ";" | expression + ";" | ifStatement;

            variableAssignment.Rule = variable + "=" + expression;
            variableDeclaration.Rule = Symbol("var") + variable;

            ifStatement.Rule = "if" + Symbol("(") + expression + Symbol(")")
                               + Symbol("{") + statementList + Symbol("}")
                               + elseStatement;
            elseStatement.Rule = Empty | "else" + Symbol("{") + statementList + Symbol("}");

            expression.Rule = number | variable | stringLiteral
                | expression + binaryOperator + expression
                | "(" + expression + ")";
        }
    }
}