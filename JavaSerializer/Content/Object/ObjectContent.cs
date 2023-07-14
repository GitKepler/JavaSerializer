using JavaSerializer.Content.Interface;
using JavaSerializer.Content.Object.ClassDesc.FieldDescriptor.Interface;
using JavaSerializer.Content.Object.Inteface;
using System.Collections.Generic;
using System.Linq;

namespace JavaSerializer.Content.Object
{
    public class ObjectContent : IObject, IObjectWithClassDescriptor
    {
        public ObjectContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public IClassDescriptor? ClassDescriptor { get; set; }
        public IDictionary<IClassField, object> Values { get; } = new Dictionary<IClassField, object>();

        public void GenerateFieldMapping()
        {
            Fields = Values?.Keys?.ToDictionary(key => key.Name, key => key);
        }

        public IReadOnlyDictionary<string, IClassField>? Fields { get; private set; }
    }
}