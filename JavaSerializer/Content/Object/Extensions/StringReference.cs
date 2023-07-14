using JavaSerializer.Content.Object.Inteface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JavaSerializer.Content.Object.Extensions
{
    public class StringReference : ReferenceContent, IString
    {
        public StringReference(TokenType contentType, IString referenceValue) : base(contentType, referenceValue)
        {
        }

        public IString? Value => PointerValue as IString;

        public string? FinalString
        {
            get => Value?.FinalString;
            set
            {
                if (Value is null)
                    throw new InvalidDataException("This string reference is not pointing to anything.");
                Value.FinalString = value;
            }
        }
    }
}
