using JavaSerializer.Content.Interface;

namespace JavaSerializer.Content.Object
{
    public class ResetContent : IObject
    {
        public ResetContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
    }
}