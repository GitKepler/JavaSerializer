using JavaSerializer.Content.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace JavaSerializer.Content.Object.Inteface
{
    public interface IString : IObject
    {
        string? FinalString { get; set; }
    }
}
