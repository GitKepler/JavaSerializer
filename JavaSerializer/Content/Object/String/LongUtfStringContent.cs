using JavaSerializer.Content.Object.String.Interface;

namespace JavaSerializer.Content.Object.String
{
    public class LongUtfStringContent : IStringContent
    {
        public LongUtfStringContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public string? String { get; set; }
    }
}