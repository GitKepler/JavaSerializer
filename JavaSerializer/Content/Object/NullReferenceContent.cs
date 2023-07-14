using JavaSerializer.Content.Interface;
using JavaSerializer.Content.Object.Inteface;

namespace JavaSerializer.Content.Object
{
    public class NullReferenceContent : IObject, IClassDescriptor
    {
        public NullReferenceContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
    }
}