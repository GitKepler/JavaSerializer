using JavaSerializer.Content.Interface;

namespace JavaSerializer.Content.Object
{
    public class NullReferenceContent : IObject
    {
        public NullReferenceContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
    }
}