using System;
using System.Linq.Expressions;
using Irony.Compiler;
using IExpressionGenerator = ExpressionCompiler.Common.IExpressionGenerator;

namespace ExpressionCompiler
{
    public class Compiler
    {
        public static decimal? Compile(string sourceCode)
        {
            // create a compiler from the grammar
            FlGrammar grammar = new FlGrammar();
            LanguageCompiler compiler = new LanguageCompiler(grammar);

            // Attempt to compile into an Abstract Syntax Tree. Because FLGrammar
            // defines the root node as ProgramNode, that is what will be returned.
            // This happens to implement IJavaScriptGenerator, which is what we need.
            IExpressionGenerator program = (IExpressionGenerator)compiler.Parse(sourceCode);
            if (program == null || compiler.Context.Errors.Count > 0)
            {
                // Didn't compile.  Generate an error message.
                SyntaxError error = compiler.Context.Errors[0];
                string location = string.Empty;
                if (error.Location.Line > 0 && error.Location.Column > 0)
                {
                    location = "Line " + (error.Location.Line + 1) + ", column " + (error.Location.Column + 1);
                }
                string message = location + ": " + error.Message + ":" + Environment.NewLine;
                message += sourceCode.Split('\n')[error.Location.Line];

                throw new CompilationException(message);
            }

            // now just instruct the compilation of to javascript
            //StringBuilder js = new StringBuilder();

            var expression = program.GenerateExpression(null);
            return ((Expression<Func<decimal?>>)expression).Compile()();
        }

    }

    public class CompilationException : Exception
    {
        public CompilationException(string message)
            : base(message)
        {

        }
    }
}