using JavaSerializer.Content.Interface;
using JavaSerializer.Content.Object.ClassDesc.FieldDescriptor.Interface;
using JavaSerializer.Content.Object.Inteface;

namespace JavaSerializer.Content.Object.ClassDesc.FieldDescriptor
{
    public class ObjectField : IClassField
    {
        public ObjectField(FieldType fieldType, string fieldName, IString fieldDescriptorString)
        {
            Type = fieldType;
            Name = fieldName;
            UnderlyingType = fieldDescriptorString;
        }

        public FieldType Type { get; }
        public string Name { get; }
        public IString UnderlyingType { get; }
    }
}