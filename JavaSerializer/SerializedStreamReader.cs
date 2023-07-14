using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JavaSerializer.Content;
using JavaSerializer.Content.BlockData;
using JavaSerializer.Content.Interface;
using JavaSerializer.Content.Object;
using JavaSerializer.Content.Object.ClassDesc;
using JavaSerializer.Content.Object.ClassDesc.FieldDescriptor;
using JavaSerializer.Content.Object.ClassDesc.FieldDescriptor.Interface;
using JavaSerializer.Content.Object.String;

namespace JavaSerializer
{
    public class SerializedStreamReader : IDisposable
    {
        private readonly BinaryReader _reader;

        public ushort Version { get; private set; }
        public IReadOnlyList<IContent> Content => _content;
        private readonly List<IContent> _content = new();

        public SerializedStreamReader(Stream stream)
        {
            _reader = new BinaryReader(stream, Encoding.UTF8);
        }

        public void Read()
        {
            var magic = _reader.ReadUInt16BE();
            if (magic != 0xACED) throw new InvalidDataException($"Invalid magic number: Received 0x{magic:X4}, Expected 0xACED.");
            Version = _reader.ReadUInt16BE();

            while (ReadContent(out var content, false, TokenType.TC_ENDBLOCKDATA) && content is not null)
            {
                _content.Add(content);
            }
        }

        private readonly List<IContent> _handleMapping = new();
        public IReadOnlyList<IContent> HandleMapping => _handleMapping;

        private const uint HandleOffset = 0x007E0000;

        private bool ReadContent(out IContent? content, bool whitelistRestriction, params TokenType[] restrictions)
        {
            TokenType contentType;
            try
            {
                contentType = _reader.ReadTokenType();
            }
            catch (EndOfStreamException)
            {
                content = null;
                return false;
            }

            if (restrictions is not null && (whitelistRestriction ^ restrictions.Contains(contentType)))
                throw new InvalidDataException($"An invalid content has been read. Fetched content type = {contentType}.");

            switch (contentType)
            {
                case TokenType.TC_OBJECT: // done
                    var objectContent = new ObjectContent(contentType);
                    ReadObject(objectContent);
                    content = objectContent;
                    break;
                case TokenType.TC_CLASS: // done
                    var classContent = new ClassContent(contentType);
                    ReadClass(classContent);
                    content = classContent;
                    break;
                case TokenType.TC_ARRAY:
                    var arrayContent = new ArrayContent(contentType);
                    ReadArray(arrayContent);
                    content = arrayContent;
                    break;
                case TokenType.TC_STRING: // done
                    var stringContent = new UtfStringContent(contentType);
                    ReadString(stringContent);
                    content = stringContent;
                    break;
                case TokenType.TC_LONGSTRING: // done
                    var longStringContent = new LongUtfStringContent(contentType);
                    ReadString(longStringContent);
                    content = longStringContent;
                    break;
                case TokenType.TC_ENUM: // done
                    var enumContent = new EnumContent(contentType);
                    ReadEnum(enumContent);
                    content = enumContent;
                    break;
                case TokenType.TC_CLASSDESC: // done
                    var classDescContent = new ClassDescriptorContent(contentType);
                    ReadClassDescriptor(classDescContent);
                    content = classDescContent;
                    break;
                case TokenType.TC_PROXYCLASSDESC: // done
                    var proxyDescContent = new ProxyClassDescContent(contentType);
                    ReadProxyClassDescriptor(proxyDescContent);
                    content = proxyDescContent;
                    break;
                case TokenType.TC_REFERENCE: // done
                    var localReference = new ReferenceContent(contentType);
                    var handleValue = (int)(_reader.ReadUInt32BE() - HandleOffset);
                    localReference.PointerValue = HandleMapping[handleValue];
                    content = localReference;
                    break;
                case TokenType.TC_NULL: // done
                    content = new NullReferenceContent(contentType);
                    break;
                case TokenType.TC_EXCEPTION:
                    content = new ExceptionContent(contentType);
                    break;
                case TokenType.TC_RESET: // done
                    content = new ResetContent(contentType);
                    break;
                case TokenType.TC_BLOCKDATA: // done
                    var blockDataContent = new BlockDataShortContent(contentType);
                    blockDataContent.Size = _reader.ReadByte();
                    blockDataContent.Data = _reader.ReadBytes(blockDataContent.Size);
                    content = blockDataContent;
                    break;
                case TokenType.TC_BLOCKDATALONG: // done
                    var blockDataLongContent = new BlockDataLongContent(contentType);
                    blockDataLongContent.Size = _reader.ReadInt32BE();
                    blockDataLongContent.Data = _reader.ReadBytes(blockDataLongContent.Size);
                    content = blockDataLongContent;
                    break;
                case TokenType.TC_ENDBLOCKDATA: // done
                    content = new EndBlockDataContent(contentType);
                    break;
                default: throw new InvalidDataException($"Invalid content type received: {contentType}");
            }

            return true;
        }

        private void ReadArray(ArrayContent content)
        {
            _ = ReadContent(out var classDescriptor, true, TokenType.TC_CLASSDESC, TokenType.TC_PROXYCLASSDESC, TokenType.TC_NULL, TokenType.TC_REFERENCE);
            if (classDescriptor is null) throw new EndOfStreamException("End of stream reached");
            content.ClassDescriptor = classDescriptor;
            _handleMapping.Add(content);

            int size = _reader.ReadInt32BE();
            content.Data = new object[size];

            while (classDescriptor?.Header == TokenType.TC_REFERENCE && classDescriptor is ReferenceContent reference)
            {
                classDescriptor = reference.PointerValue;
            }

            if (classDescriptor?.Header == TokenType.TC_NULL) throw new InvalidDataException("The class descriptor is a null reference.");
            if (classDescriptor?.Header == TokenType.TC_PROXYCLASSDESC) throw new InvalidDataException("Proxy class descriptors are not supported when fetching class members.");

            if (classDescriptor?.Header != TokenType.TC_CLASSDESC || classDescriptor is not ClassDescriptorContent finalClassDescriptor) throw new InvalidDataException($"The object is not a class descriptor but a {classDescriptor?.Header}.");

            if(finalClassDescriptor.ClassName?.Length == 2 && finalClassDescriptor.ClassName[0] == '[')
            {
                var fieldTypeValue = (byte)finalClassDescriptor.ClassName[1];
                if (!Enum.IsDefined(typeof(FieldType), fieldTypeValue)) throw new InvalidDataException($"Unknown field type: {finalClassDescriptor.ClassName}");

                var fieldType = (FieldType)fieldTypeValue;

                for (int i = 0; i < size; i++)
                {
                    content.Data[i] = fieldType switch
                    {
                        FieldType.Byte => _reader.ReadByte(),
                        FieldType.Char => _reader.ReadChar(),
                        FieldType.Double => _reader.ReadDouble(),
                        FieldType.Float => _reader.ReadSingle(),
                        FieldType.Integer => _reader.ReadInt32BE(),
                        FieldType.Long => _reader.ReadInt64BE(),
                        FieldType.Short => _reader.ReadInt16BE(),
                        FieldType.Boolean => _reader.ReadBoolean(),
                        FieldType.Array or FieldType.Object => throw new InvalidDataException($"A {fieldType} is not a primitive field type."),
                        _ => throw new InvalidDataException($"Unknown value provided ({fieldType})")
                    };
                }
            }
            else
            {
                for(int i = 0; i < size; i++)
                {
                    _ = ReadContent(out var resultingObject, true, TokenType.TC_NULL, TokenType.TC_REFERENCE, TokenType.TC_ARRAY, TokenType.TC_OBJECT, TokenType.TC_STRING, TokenType.TC_LONGSTRING);
                    if (resultingObject is null) throw new EndOfStreamException("End of stream reached");

                    content.Data[i] = resultingObject;
                }
            }
        }

        private void ReadObject(ObjectContent content)
        {
            _ = ReadContent(out var classDescriptor, true, TokenType.TC_CLASSDESC, TokenType.TC_PROXYCLASSDESC, TokenType.TC_NULL, TokenType.TC_REFERENCE, TokenType.TC_STRING, TokenType.TC_LONGSTRING);
            if (classDescriptor is null) throw new EndOfStreamException("End of stream reached");
            content.ClassDescriptor = classDescriptor;
            _handleMapping.Add(content);

            foreach(var field in GetClassFieldsFromClassDescriptor(classDescriptor))
            {
                content.Values ??= new Dictionary<IClassField, object>();

                if(field is PrimitiveField primitiveField)
                {
                    content.Values[primitiveField] = primitiveField.Type switch
                    {
                        FieldType.Byte => _reader.ReadByte(),
                        FieldType.Char => _reader.ReadChar(),
                        FieldType.Double => _reader.ReadDouble(),
                        FieldType.Float => _reader.ReadSingle(),
                        FieldType.Integer => _reader.ReadInt32BE(),
                        FieldType.Long => _reader.ReadInt64BE(),
                        FieldType.Short => _reader.ReadInt16BE(),
                        FieldType.Boolean => _reader.ReadBoolean(),
                        FieldType.Array or FieldType.Object => throw new InvalidDataException($"A {primitiveField.Type} is not a primitive field type."),
                        _ => throw new InvalidDataException($"Unknown value provided ({primitiveField.Type})")
                    };
                }
                else if(field is ObjectField objectField)
                {
                    _ = ReadContent(out var resultingObject, true, TokenType.TC_NULL, TokenType.TC_REFERENCE, TokenType.TC_ARRAY, TokenType.TC_OBJECT, TokenType.TC_LONGSTRING, TokenType.TC_STRING);
                    if (resultingObject is null) throw new EndOfStreamException("End of stream reached");

                    content.Values[objectField] = resultingObject;
                }
                else
                {
                    throw new InvalidDataException("The provided object is not valid (unknown field type)");
                }
            }
        }

        private IReadOnlyList<IClassField> GetClassFieldsFromClassDescriptor(IContent? classDescriptorOrPointer)
        {
            var fields = new List<IList<IClassField>>();

            while(classDescriptorOrPointer?.Header == TokenType.TC_REFERENCE && classDescriptorOrPointer is ReferenceContent reference)
            {
                classDescriptorOrPointer = reference.PointerValue;
            }

            if (classDescriptorOrPointer?.Header == TokenType.TC_NULL) throw new InvalidDataException("The class descriptor is a null reference.");
            if (classDescriptorOrPointer?.Header == TokenType.TC_PROXYCLASSDESC) throw new InvalidDataException("Proxy class descriptors are not supported when fetching class members.");

            if (classDescriptorOrPointer?.Header != TokenType.TC_CLASSDESC || classDescriptorOrPointer is not ClassDescriptorContent classDescriptor) throw new InvalidDataException($"The object is not a class descriptor but a {classDescriptorOrPointer?.Header}.");

            if(classDescriptor.Fields is not null)
                fields.Add(classDescriptor.Fields);

            var superClassDescriptor = classDescriptor.SuperClassDescriptor;

            while (superClassDescriptor?.Header == TokenType.TC_REFERENCE || superClassDescriptor?.Header == TokenType.TC_CLASSDESC)
            {
                while(superClassDescriptor?.Header == TokenType.TC_REFERENCE && superClassDescriptor is ReferenceContent referenceSuper)
                {
                    superClassDescriptor = referenceSuper.PointerValue;
                }

                if(superClassDescriptor?.Header == TokenType.TC_CLASSDESC && superClassDescriptor is ClassDescriptorContent classDescriptorSuperClass)
                {
                    if (classDescriptorSuperClass.Fields is not null)
                        fields.Add(classDescriptorSuperClass.Fields);
                    classDescriptor = classDescriptorSuperClass;
                    superClassDescriptor = classDescriptorSuperClass.SuperClassDescriptor;
                }
                else
                {
                    throw new InvalidDataException($"A class has a superclass with a non-classdesc/reference entity: class \"{classDescriptor.ClassName}\", super class type: {superClassDescriptor?.Header.ToString() ?? "null"}.");
                }
            }

            fields.Reverse();
            return fields.SelectMany(x => x).ToList();
        }

        private void ReadString(LongUtfStringContent content)
        {
            _handleMapping.Add(content);

            content.String = _reader.ReadInt64String();
        }

        private void ReadString(UtfStringContent content)
        {
            _handleMapping.Add(content);

            content.String = _reader.ReadUInt16String();
        }

        private void ReadEnum(EnumContent content)
        {
            _ = ReadContent(out var classDescriptor, true, TokenType.TC_CLASSDESC, TokenType.TC_PROXYCLASSDESC, TokenType.TC_NULL, TokenType.TC_REFERENCE);
            if (classDescriptor is null) throw new EndOfStreamException("End of stream reached");
            content.ClassDescriptor = classDescriptor;

            _handleMapping.Add(content);

            _ = ReadContent(out var stringContent, true, TokenType.TC_STRING, TokenType.TC_LONGSTRING, TokenType.TC_REFERENCE);
            if (stringContent is null) throw new EndOfStreamException("End of stream reached");
            content.EnumConstantName = stringContent;
        }

        private void ReadClass(ClassContent content)
        {
            _ = ReadContent(out var classDescriptor, true, TokenType.TC_CLASSDESC, TokenType.TC_PROXYCLASSDESC, TokenType.TC_NULL, TokenType.TC_REFERENCE);
            if (classDescriptor is null) throw new EndOfStreamException("End of stream reached");
            content.ClassDescriptor = classDescriptor;

            _handleMapping.Add(content);
        }

        private void ReadProxyClassDescriptor(ProxyClassDescContent content)
        {
            _handleMapping.Add(content);

            var interfaceNameCount = _reader.ReadUInt32BE();
            if(interfaceNameCount > 0)
            {
                content.InterfaceNames = new List<string>();
                for(int i = 0; i < interfaceNameCount; i++)
                {
                    content.InterfaceNames.Add(_reader.ReadUInt16String());
                }
            }

            while (ReadContent(out var annotation, false /* Allow all types */) && annotation is not EndBlockDataContent && annotation is not null)
            {
                content.Annotations ??= new List<IContent>();
                content.Annotations.Add(annotation);
            }

            _ = ReadContent(out var superClassDescriptor, true, TokenType.TC_CLASSDESC, TokenType.TC_PROXYCLASSDESC, TokenType.TC_NULL, TokenType.TC_REFERENCE);
            if (superClassDescriptor is null) throw new EndOfStreamException("End of stream reached");
            content.SuperClassDescriptor = superClassDescriptor;
        }

        private void ReadClassDescriptor(ClassDescriptorContent content)
        {
            content.ClassName = _reader.ReadUInt16String();
            content.SerialVersionUID = _reader.ReadUInt64BE();
            _handleMapping.Add(content);

            content.ClassDescFlag = _reader.ReadClassDescFlags();
            var fieldCount = _reader.ReadUInt16BE();
            if (fieldCount > 0)
            {
                content.Fields = new List<IClassField>();
                for (int i = 0; i < fieldCount; i++)
                {
                    content.Fields.Add(ReadField());
                }
            }

            while(ReadContent(out var annotation, false /* Allow all types */) && annotation is not EndBlockDataContent && annotation is not null)
            {
                content.Annotations ??= new List<IContent>();
                content.Annotations.Add(annotation);
            }

            _ = ReadContent(out var superClassDescriptor, true, TokenType.TC_CLASSDESC, TokenType.TC_PROXYCLASSDESC, TokenType.TC_NULL, TokenType.TC_REFERENCE);
            if (superClassDescriptor is null) throw new EndOfStreamException("End of stream reached");
            content.SuperClassDescriptor = superClassDescriptor;
        }

        private IClassField ReadField()
        {
            var fieldType = _reader.ReadFieldType();
            var fieldName = _reader.ReadUInt16String();
            if(fieldType == FieldType.Array || fieldType == FieldType.Object)
            {
                _ = ReadContent(out var stringContent, true, TokenType.TC_STRING, TokenType.TC_LONGSTRING, TokenType.TC_REFERENCE);
                if (stringContent is null) throw new EndOfStreamException("End of stream reached");

                return new ObjectField(fieldType, fieldName, stringContent);
            }

            return new PrimitiveField(fieldType, fieldName);
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }

    public class SerializedStreamWriter
    {
    }
}
