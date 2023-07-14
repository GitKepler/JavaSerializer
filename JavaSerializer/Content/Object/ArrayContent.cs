using JavaSerializer.Content.Interface;

namespace JavaSerializer.Content.Object
{
    public class ArrayContent : IObject
    {
        public ArrayContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public IContent? ClassDescriptor { get; set; }
        public object[]? Data { get; set; }
    }
}