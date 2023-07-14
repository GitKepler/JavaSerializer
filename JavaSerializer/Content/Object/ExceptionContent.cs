using JavaSerializer.Content.Interface;

namespace JavaSerializer.Content.Object
{
    public class ExceptionContent : IObject
    {
        public ExceptionContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
    }
}