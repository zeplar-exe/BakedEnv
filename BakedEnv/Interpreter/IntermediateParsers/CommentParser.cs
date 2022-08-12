using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

using TokenCs;
using TokenCs.Extensions;

namespace BakedEnv.Interpreter.IntermediateParsers;

public class CommentParser : MatchParser
{
    public override bool Match(LexerToken first)
    {
        return TestTokenIs(first, LexerTokenType.Hashtag);
    }

    public override IntermediateToken Parse(LexerToken first, ParserIterator iterator)
    {
        var token = new SingleLineCommentToken();
        
        while (iterator.TryMoveNext(out var next))
        {
            if (token.Content.Count == 0)
            {
                if (next.Type == LexerTokenType.Hashtag)
                {
                    var multiToken = new MultiLineCommentToken
                    {
                        Start = new MultiLineCommentDelimiterToken
                        {
                            FirstHash = new HashToken(first),
                            SecondHash = new HashToken(next)
                        }
                    };

                    return ParseMultiLine(multiToken, iterator);
                }
            }
            
            var any = new AnyToken(next);
            
            token.Content.Add(any);
            
            if (next.IsNewLine())
                break;
        }

        return token.AsComplete();
    }

    private MultiLineCommentToken ParseMultiLine(MultiLineCommentToken target, ParserIterator iterator)
    {
        void AppendContent(LexerToken token)
        {
            var any = new AnyToken(token);
            
            target.Content.Add(any);
        }
        
        while (iterator.TryMoveNext(out var next))
        {
            if (next.Type == LexerTokenType.Hashtag)
            {
                if (iterator.TryMoveNext(out var afterNext)) 
                    // If false, we skip down and append next anyway
                    // Will subsequently end the loop and return as incomplete
                {
                    if (afterNext.Type == LexerTokenType.Hashtag)
                    {
                        target.End = new MultiLineCommentDelimiterToken
                        {
                            FirstHash = new HashToken(next),
                            SecondHash = new HashToken(afterNext)
                        };

                        return target.AsComplete();
                    }
                    else
                    {
                        AppendContent(next);
                        AppendContent(afterNext);

                        continue;
                    }
                }
            }
            
            AppendContent(next);
        }

        return target.AsIncomplete();
    }
}