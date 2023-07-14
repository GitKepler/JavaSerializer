using JavaSerializer.Content.Interface;
using JavaSerializer.Content.Object.Inteface;

namespace JavaSerializer.Content.Object
{
    public class UtfStringContent : IObject, IString
    {
        public UtfStringContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public string? String { get; set; }
    }
}