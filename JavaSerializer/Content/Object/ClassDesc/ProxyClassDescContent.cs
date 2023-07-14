using JavaSerializer.Content.Interface;
using JavaSerializer.Content.Object.ClassDesc.Interface;
using JavaSerializer.Content.Object.Inteface;
using System.Collections.Generic;

namespace JavaSerializer.Content.Object.ClassDesc
{
    public class ProxyClassDescContent : IClassDesciptorContent
    {
        public ProxyClassDescContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public IList<string>? InterfaceNames { get; set; }
        public IList<IContent>? Annotations { get; set; }
        public IClassDescriptor? ClassDescriptor { get; set; }
    }
}