using JavaSerializer.Content.Object.Inteface;
using System;
using System.Collections.Generic;
using System.Text;

namespace JavaSerializer.Content.Object.Extensions
{
    public class StringReference : ReferenceContent, IString
    {
        public StringReference(TokenType contentType, IString referenceValue) : base(contentType, referenceValue)
        {
        }

        public IString? Value => PointerValue as IString;
    }
}
