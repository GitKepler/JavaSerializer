using JavaSerializer.Content.Interface;

namespace JavaSerializer.Content.Object
{
    public class UtfStringContent : IObject
    {
        public UtfStringContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public string? String { get; set; }
    }
}