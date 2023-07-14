using JavaSerializer.Content.Interface;
using JavaSerializer.Content.Object.Inteface;

namespace JavaSerializer.Content.Object
{
    public class ArrayContent : IObject
    {
        public ArrayContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public IClassDescriptor? ClassDescriptor { get; set; }
        public object[]? Data { get; set; }
    }
}