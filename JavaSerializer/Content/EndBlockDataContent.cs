using JavaSerializer.Content.Interface;

namespace JavaSerializer.Content
{
    public class EndBlockDataContent : IContent
    {
        public EndBlockDataContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
    }
}