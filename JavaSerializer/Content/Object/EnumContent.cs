using JavaSerializer.Content.Interface;

namespace JavaSerializer.Content.Object
{
    public class EnumContent : IObject
    {
        public EnumContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public string? EnumConstantName { get; set; }
        public IContent? ClassDescriptor { get; set; }
    }
}