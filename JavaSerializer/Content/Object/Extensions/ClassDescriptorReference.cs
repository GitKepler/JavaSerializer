using JavaSerializer.Content.Object.Inteface;
using System;
using System.Collections.Generic;
using System.Text;

namespace JavaSerializer.Content.Object.Extensions
{
    public class ClassDescriptorReference : ReferenceContent, IClassDescriptor
    {
        public ClassDescriptorReference(TokenType contentType, IClassDescriptor referenceValue) : base(contentType, referenceValue)
        {
        }

        public IClassDescriptor? Value => PointerValue as IClassDescriptor;
    }
}
