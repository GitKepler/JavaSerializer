﻿using JavaSerializer.Content.Interface;
using JavaSerializer.Content.Object.ClassDesc.FieldDescriptor.Interface;
using JavaSerializer.Content.Object.ClassDesc.Interface;
using System.Collections.Generic;

namespace JavaSerializer.Content.Object.ClassDesc
{
    public class ClassDescriptorContent : IClassDescContent
    {
        public ClassDescriptorContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public string? ClassName { get; set; }
        public ulong SerialVersionUID { get; set; }
        public ClassDescFlag? ClassDescFlag { get; set; }
        public IList<IClassField>? Fields { get; set; }
        public IList<IContent>? Annotations { get; set; }
        public IContent? SuperClassDescriptor { get; set; }
    }
}