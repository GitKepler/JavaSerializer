using JavaSerializer.Content.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace JavaSerializer.Content.Object.Extensions
{
    public class FreeReference : ReferenceContent
    {
        public FreeReference(TokenType contentType, IContent referenceContent) : base(contentType, referenceContent)
        {
        }
    }
}
