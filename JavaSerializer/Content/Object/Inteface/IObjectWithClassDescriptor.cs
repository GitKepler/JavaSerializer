using JavaSerializer.Content.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace JavaSerializer.Content.Object.Inteface
{
    public interface IObjectWithClassDescriptor : IObject
    {
        IClassDescriptor? ClassDescriptor { get; set; }
    }
}
