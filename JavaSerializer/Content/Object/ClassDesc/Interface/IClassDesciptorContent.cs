using JavaSerializer.Content.Interface;
using JavaSerializer.Content.Object.Inteface;
using System.Collections.Generic;

namespace JavaSerializer.Content.Object.ClassDesc.Interface
{
    public interface IClassDesciptorContent : IObject, IClassDescriptor
    {
        IClassDescriptor? SuperClassDescriptor { get; set; }
        IList<IContent>? Annotations { get; set; }
    }
}