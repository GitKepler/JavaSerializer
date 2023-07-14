using JavaSerializer.Content.Object.String.Interface;

namespace JavaSerializer.Content.Object.String
{
    public class UtfStringContent : IStringContent
    {
        public UtfStringContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public string? String { get; set; }
    }
}