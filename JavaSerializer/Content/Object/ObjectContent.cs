using JavaSerializer.Content.Interface;
using JavaSerializer.Content.Object.ClassDesc.FieldDescriptor.Interface;
using System.Collections.Generic;

namespace JavaSerializer.Content.Object
{
    public class ObjectContent : IObject
    {
        public ObjectContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public IContent? ClassDescriptor { get; set; }
        public IDictionary<IClassField, object>? Values { get; set; }
    }
}