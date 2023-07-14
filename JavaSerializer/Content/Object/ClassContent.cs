using JavaSerializer.Content.Interface;
using JavaSerializer.Content.Object.Inteface;

namespace JavaSerializer.Content.Object
{
    public class ClassContent : IObject, IObjectWithClassDescriptor
    {
        public ClassContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public IClassDescriptor? ClassDescriptor { get; set; }
    }
}