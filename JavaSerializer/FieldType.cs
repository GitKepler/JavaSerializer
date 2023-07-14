namespace JavaSerializer
{
    public enum FieldType : byte
    {
        None,

        Byte = 66,
        Char = 67,
        Double = 68,
        Float = 70,
        Integer = 73,
        Long = 74,
        Short = 83,
        Boolean = 90,

        Array = 91,
        Object = 76
    }
}
