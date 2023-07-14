using JavaSerializer.Content.Interface;
using JavaSerializer.Content.Object.Inteface;

namespace JavaSerializer.Content.Object
{
    public class EnumContent : IObject, IObjectWithClassDescriptor
    {
        public EnumContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public IContent? EnumConstantName { get; set; }
        public IClassDescriptor? ClassDescriptor { get; set; }
    }
}