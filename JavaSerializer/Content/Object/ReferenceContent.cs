using JavaSerializer.Content.Interface;

namespace JavaSerializer.Content.Object
{
    public abstract class ReferenceContent : IObject
    {
        protected ReferenceContent(TokenType contentType, IContent pointerValue)
        {
            Header = contentType;
            PointerValue = pointerValue;
        }

        public TokenType Header { get; }
        public IContent PointerValue { get; }
    }
}