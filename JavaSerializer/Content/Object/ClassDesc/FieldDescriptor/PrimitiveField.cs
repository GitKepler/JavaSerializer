using JavaSerializer.Content.Object.ClassDesc.FieldDescriptor.Interface;

namespace JavaSerializer.Content.Object.ClassDesc.FieldDescriptor
{
    public class PrimitiveField : IClassField
    {
        public PrimitiveField(FieldType fieldType, string fieldName)
        {
            Type = fieldType;
            Name = fieldName;
        }

        public FieldType Type { get; }
        public string Name { get; }
    }
}