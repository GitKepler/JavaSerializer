using JavaSerializer.Content.Interface;

namespace JavaSerializer.Content.Object
{
    public class ClassContent : IObject
    {
        public ClassContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public IContent? ClassDescriptor { get; set; }
    }
}