using System;

namespace JavaSerializer
{
    [Flags]
    public enum ClassDescFlag : byte
    {
        None = 0,
        SC_WRITE_METHOD = 0x01,
        SC_SERIALIZABLE = 1 << 1,
        SC_EXTERNALIZABLE = 1 << 2,
        SC_BLOCK_DATA = 1 << 3,
        SC_ENUM = 1 << 4
    }
}
