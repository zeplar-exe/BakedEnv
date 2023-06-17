using System.Collections;

namespace BakedEnv.Interpreter.Lexer;

public class TextLexer : IEnumerable<TextualToken>, IDisposable
{
    private ulong Index { get; set; }
    private bool ReserveCurrent { get; set; }
    private IEnumerator<char> Source { get; }

    public TextLexer(IEnumerable<char> source)
    {
        Source = source.GetEnumerator();
    }

    public IEnumerator<TextualToken> GetEnumerator()
    {
        while (ReserveCurrent || Source.MoveNext())
        {
            ReserveCurrent = false;
            
            var next = Source.Current;
            var startIndex = Index;

            TextualToken CreateToken(string text, TextualTokenType type)
            {
                return new TextualToken(text, type, startIndex);
            }

            if (char.IsLetter(next))
            {
                var alpha = string.Concat(EnumerateWhile(char.IsLetter).Prepend(next));

                yield return CreateToken(alpha, TextualTokenType.Alphabetic);
            }
            else if (char.IsDigit(next))
            {
                var num = string.Concat(EnumerateWhile(char.IsDigit).Prepend(next));

                yield return CreateToken(num, TextualTokenType.Numeric);
            }
            else if (next == ' ')
            {
                var space = string.Concat(EnumerateWhile(c => c == ' ').Prepend(next));

                yield return CreateToken(space, TextualTokenType.Space);
            }
            else if (next == '\r')
            {
                if (TryNext(out var after))
                {
                    if (after == '\n')
                    {
                        var text = next.ToString() + after;
                        
                        yield return CreateToken(text, TextualTokenType.NewLine);
                    }
                    else
                    {
                        yield return CreateToken(next.ToString(), TextualTokenType.NewLine);
                        
                        ReserveCurrent = true;
                    }
                }
                else
                {
                    yield return CreateToken(next.ToString(), TextualTokenType.NewLine);
                }
            }
            else
            {
                var type = next switch
                {
                    '~' => TextualTokenType.Tilde,
                    '`' => TextualTokenType.Backtick,
                    '!' => TextualTokenType.ExclamationMark,
                    '@' => TextualTokenType.At,
                    '#' => TextualTokenType.Hashtag,
                    '$' => TextualTokenType.Dollar,
                    '%' => TextualTokenType.Percent,
                    '^' => TextualTokenType.Caret,
                    '&' => TextualTokenType.Ampersand,
                    '*' => TextualTokenType.Star,
                    '(' => TextualTokenType.LeftParenthesis,
                    ')' => TextualTokenType.RightParenthesis,
                    '_' => TextualTokenType.Underscore,
                    '-' => TextualTokenType.Dash,
                    '+' => TextualTokenType.Plus,
                    '=' => TextualTokenType.Equals,
                    '{' => TextualTokenType.LeftCurlyBracket,
                    '[' => TextualTokenType.LeftBracket,
                    '}' => TextualTokenType.RightCurlyBracket,
                    ']' => TextualTokenType.RightBracket,
                    '|' => TextualTokenType.Vertical,
                    '\\' => TextualTokenType.Backslash,
                    ':' => TextualTokenType.Colon,
                    ';' => TextualTokenType.Semicolon,
                    '\"' => TextualTokenType.DoubleQuotation,
                    '\'' => TextualTokenType.SingleQuotation,
                    '<' => TextualTokenType.LessThan,
                    '>' => TextualTokenType.GreaterThan,
                    ',' => TextualTokenType.Comma,
                    '.' => TextualTokenType.Period,
                    '?' => TextualTokenType.QuestionMark,
                    '/' => TextualTokenType.Slash,
                    ' ' => TextualTokenType.Space,
                    '\t' => TextualTokenType.Tab,
                    '\r' => TextualTokenType.NewLine,
                    '\n' => TextualTokenType.NewLine,
                    _ => TextualTokenType.Unhandled
                };

                yield return CreateToken(next.ToString(), type);
            }
        }
    }

    private IEnumerable<char> EnumerateWhile(Predicate<char> predicate)
    {
        while (TryNext(out var next))
        {
            if (!predicate.Invoke(next))
            {
                ReserveCurrent = true;
                
                break;
            }

            yield return next;
        }
    }

    private bool TryNext(out char next)
    {
        next = default;
        
        if (!Source.MoveNext())
            return false;

        if (!ReserveCurrent)
            Index++;

        ReserveCurrent = false;
        next = Source.Current;

        return true;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public void Dispose()
    {
        Source.Dispose();
    }
}