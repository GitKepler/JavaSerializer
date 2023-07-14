namespace JavaSerializer
{
    public enum TokenType : byte
    {
        TC_NULL = 0x70,
        TC_REFERENCE = 0x71,
        TC_CLASSDESC = 0x72,
        TC_OBJECT = 0x73,
        TC_STRING = 0x74,
        TC_ARRAY = 0x75,
        TC_CLASS = 0x76,
        TC_BLOCKDATA = 0x77,
        TC_ENDBLOCKDATA = 0x78,
        TC_RESET = 0x79,
        TC_BLOCKDATALONG = 0x7A,
        TC_EXCEPTION = 0x7B,
        TC_LONGSTRING = 0x7C,
        TC_PROXYCLASSDESC = 0x7D,
        TC_ENUM = 0x7E
    }
}
