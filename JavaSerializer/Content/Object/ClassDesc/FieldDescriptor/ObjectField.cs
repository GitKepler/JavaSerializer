using JavaSerializer.Content.Object.ClassDesc.FieldDescriptor.Interface;
using JavaSerializer.Content.Object.String.Interface;

namespace JavaSerializer.Content.Object.ClassDesc.FieldDescriptor
{
    public class ObjectField : IClassField
    {
        public ObjectField(FieldType fieldType, string fieldName, string fieldDescriptorString)
        {
            Type = fieldType;
            Name = fieldName;
            UnderlyingType = fieldDescriptorString;
        }

        public FieldType Type { get; }
        public string Name { get; }
        public string UnderlyingType { get; }
    }
}