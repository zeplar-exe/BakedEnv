using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateParsers;

public class CommentIntermediateParser : MatchIntermediateParser
{
    public override bool Match(TextualToken first)
    {
        return TestTokenIs(first, TextualTokenType.Hashtag);
    }

    public override IntermediateToken Parse(TextualToken first, LexerIterator iterator)
    {
        var token = new SingleLineCommentToken();
        
        while (iterator.TryMoveNext(out var next))
        {
            if (token.Content.Count == 0)
            {
                if (next.Type == TextualTokenType.Hashtag)
                {
                    var multiToken = new MultiLineCommentToken
                    {
                        Start = new MultiLineCommentDelimiterToken
                        {
                            FirstHash = new RawIntermediateToken(first),
                            SecondHash = new RawIntermediateToken(next)
                        }
                    };

                    return ParseMultiLine(multiToken, iterator);
                }
            }
            
            var content = new RawIntermediateToken(next);
            
            token.Content.Add(content);
            
            if (next.Type == TextualTokenType.NewLine)
                break;
        }

        return token.AsComplete();
    }

    private MultiLineCommentToken ParseMultiLine(MultiLineCommentToken target, LexerIterator iterator)
    {
        void AppendContent(TextualToken token)
        {
            var content = new RawIntermediateToken(token);
            
            target.Content.Add(content);
        }
        
        while (iterator.TryMoveNext(out var next))
        {
            if (next.Type == TextualTokenType.Hashtag)
            {
                if (iterator.TryMoveNext(out var afterNext)) 
                    // If false, we skip down and append next anyway
                    // Will subsequently end the loop and return as incomplete
                {
                    if (afterNext.Type == TextualTokenType.Hashtag)
                    {
                        target.End = new MultiLineCommentDelimiterToken
                        {
                            FirstHash = new RawIntermediateToken(next),
                            SecondHash = new RawIntermediateToken(afterNext)
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
    
    private class MultiLineCommentDelimiterToken : IntermediateToken
    {
        public ILowLevelToken? FirstHash { get; set; }
        public ILowLevelToken? SecondHash { get; set; }

        public override long StartIndex => FirstHash!.StartIndex;
        public override long Length => FirstHash!.Length + SecondHash!.Length;
        public override long EndIndex => SecondHash!.EndIndex;
    
        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}