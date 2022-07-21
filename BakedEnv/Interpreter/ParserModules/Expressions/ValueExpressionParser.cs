using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.ParserModules.Common;
using BakedEnv.Interpreter.ParserModules.Identifiers;
using BakedEnv.Interpreter.ParserModules.Values;
using BakedEnv.Interpreter.Parsers;
using BakedEnv.Objects;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class ValueExpressionParser : ParserModule
{
    public ValueExpressionParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public ValueExpressionParserResult Parse()
    {
        var builder = new ValueExpressionParserResult.Builder();
        
        if (!Internals.Iterator.TryMoveNext(out var first))
        {
            return builder.BuildFailure();
        }
        
        Internals.Iterator.PushCurrent();

        switch (first.Type)
        {
            case LexerTokenType.AlphaNumeric: // Variable
            case LexerTokenType.Underscore:
            {
                if (first.ToString() == FunctionValueParser.Keyword)
                {
                    var functionParser = new FunctionValueParser(Internals);
                    var functionResult = functionParser.Parse();

                    if (functionResult.IsDeclaration)
                    {
                        if (!functionResult.IsComplete)
                        {
                            return builder.BuildFailure();
                        }

                        var expression = new ValueExpression(functionResult.Function);
                        
                        return builder.BuildSuccess(expression);
                    }
                }

                var identifierParser = new ChainIdentifierParser(Internals);
                var result = identifierParser.Parse();
                
                builder.AddTokensFrom(result);

                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }

                var reference = result.CreateReference(Internals.Interpreter);

                return builder.BuildSuccess(new VariableExpression(reference));
            }
            case LexerTokenType.Numeric: // Integer/Decimal
            {
                var numericParser = new NumericParser(Internals);
                var result = numericParser.Parse();

                builder.AddTokensFrom(result);
                
                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }

                return builder.BuildSuccess(new ValueExpression(result.Value));
            }
            case LexerTokenType.SingleQuotation: // String
            case LexerTokenType.DoubleQuotation:
            {
                var stringParser = new StringParser(Internals);
                var result = stringParser.Parse();

                builder.AddTokensFrom(result);

                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }
                
                var str = new BakedString(result.String);

                return builder.BuildSuccess(new ValueExpression(str));
            }
            case LexerTokenType.LeftParenthesis: // Parenthesis
            {
                Internals.IteratorTools.SkipWhitespaceAndNewlines();
                
                var expressionParser = new ExpressionParser(Internals);
                var result = expressionParser.Parse();

                builder.AddTokensFrom(result);
                
                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }
                
                if (!Internals.Iterator.TryMoveNext(out var end))
                {
                    return builder.BuildFailure();
                }

                if (end.Type != LexerTokenType.RightParenthesis)
                {
                    return builder.BuildFailure();
                }

                return builder.BuildSuccess(new ParenthesisExpression(result.Expression));
            }
            case LexerTokenType.LeftBracket:
            {
                var arrayParser = new ArrayParser(Internals);
                var arrayResult = arrayParser.Parse();

                if (!arrayResult.IsComplete)
                {
                    return builder.BuildFailure();
                }

                var values = arrayResult
                    .ExpressionList
                    .Expressions
                    .Select(e => e.Expression);
                var expression = new ArrayExpression(values);

                return builder.BuildSuccess(expression);
            }
            case LexerTokenType.LeftCurlyBracket:
            {
                var tableParser = new TableParser(Internals);
                var tableResult = tableParser.Parse();

                if (!tableResult.IsComplete)
                {
                    return builder.BuildFailure();
                }

                var keys = new List<BakedExpression>();
                var values = new List<BakedExpression>();

                foreach (var pair in tableResult.KeyValuePairs)
                {
                    keys.Add(pair.Key.Expression);
                    values.Add(pair.Value.Expression);
                }
                
                var expression = new TableExpression(keys, values);

                return builder.BuildSuccess(expression);
            }
            case LexerTokenType.Dash: // Unary negation
            {
                Internals.IteratorTools.SkipWhitespaceAndNewlines();
                
                var expressionParser = new ExpressionParser(Internals);
                var result = expressionParser.Parse();

                builder.AddTokensFrom(result);
                
                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }

                return builder.BuildSuccess(new NegateExpression(result.Expression));
            }
        }

        return builder.BuildFailure();
    }
}