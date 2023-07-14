using JavaSerializer.Content.Interface;

namespace JavaSerializer.Content.Object
{
    public class ReferenceContent : IObject
    {
        public ReferenceContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public IContent? PointerValue { get; set; }
    }
}