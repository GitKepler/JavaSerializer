using System;
using System.IO;
using System.Text;

namespace JavaSerializer
{
    internal static class BinaryHelper
    {
        public static string ReadUInt16String(this BinaryReader reader)
        {
            var stringLength = reader.ReadUInt16BE();
            var data = reader.ReadBytes(stringLength);
            return Encoding.UTF8.GetString(data);
        }

        public static string ReadInt64String(this BinaryReader reader)
        {
            var stringLength = reader.ReadInt64BE();
            if (stringLength > int.MaxValue) throw new Exception($"A string longer than {int.MaxValue} is unsupported. Current value = {stringLength}");

            var intLength = (int)stringLength;
            var data = reader.ReadBytes(intLength);
            return Encoding.UTF8.GetString(data);
        }

        public static TokenType ReadTokenType(this BinaryReader reader)
        {
            var contentHeader = reader.ReadByte();
            if (!Enum.IsDefined(typeof(TokenType), contentHeader))
            {
                throw new InvalidDataException($"An incorrect {nameof(TokenType)} has been parsed: value is 0x{contentHeader:X2}.");
            }
            return (TokenType)contentHeader;
        }

        public static ClassDescFlag ReadClassDescFlags(this BinaryReader reader)
        {
            var contentHeader = reader.ReadByte();
            if (!Enum.IsDefined(typeof(ClassDescFlag), contentHeader))
            {
                throw new InvalidDataException($"An incorrect {nameof(ClassDescFlag)} has been parsed: value is 0x{contentHeader:X2}.");
            }
            return (ClassDescFlag)contentHeader;
        }

        public static FieldType ReadFieldType(this BinaryReader reader)
        {
            var contentHeader = reader.ReadByte();
            if (!Enum.IsDefined(typeof(FieldType), contentHeader))
            {
                throw new InvalidDataException($"An incorrect {nameof(FieldType)} has been parsed: value is 0x{contentHeader:X2}.");
            }
            return (FieldType)contentHeader;
        }

        public static ushort ReadUInt16BE(this BinaryReader reader)
        {
            var valLE = reader.ReadUInt16();
            return (ushort)((valLE >> 8) | (valLE << 8));
        }

        public static short ReadInt16BE(this BinaryReader reader)
        {
            var valLE = reader.ReadUInt16();
            unchecked
            {
                return (short)((valLE >> 8) | (valLE << 8));
            }
        }

        public static uint ReadUInt32BE(this BinaryReader reader)
        {
            var valLE = reader.ReadUInt32();
            return (valLE >> 24)
                | ((valLE << 8 ) & 0x00FF0000)
                | ((valLE >> 8 ) & 0x0000FF00)
                | (valLE  << 24);
        }

        public static int ReadInt32BE(this BinaryReader reader)
        {
            var valLE = reader.ReadUInt32();
            unchecked
            {
                return (int)((valLE >> 24)
                    | ((valLE << 8) & 0x00FF0000)
                    | ((valLE >> 8) & 0x0000FF00)
                    | (valLE << 24));
            }
        }

        public static ulong ReadUInt64BE(this BinaryReader reader)
        {
            var valLE = reader.ReadUInt64();
            return (valLE >> 56)
                | ((valLE << 40) & 0x00FF000000000000)
                | ((valLE << 24) & 0x0000FF0000000000)
                | ((valLE << 8 ) & 0x000000FF00000000)
                | ((valLE >> 8 ) & 0x00000000FF000000)
                | ((valLE >> 24) & 0x0000000000FF0000)
                | ((valLE >> 40) & 0x000000000000FF00)
                | (valLE  << 56);
        }

        public static long ReadInt64BE(this BinaryReader reader)
        {
            var valLE = reader.ReadUInt64();
            unchecked
            {
                return (long)((valLE >> 56)
                | ((valLE << 40) & 0x00FF000000000000)
                | ((valLE << 24) & 0x0000FF0000000000)
                | ((valLE << 8) & 0x000000FF00000000)
                | ((valLE >> 8) & 0x00000000FF000000)
                | ((valLE >> 24) & 0x0000000000FF0000)
                | ((valLE >> 40) & 0x000000000000FF00)
                | (valLE << 56));
            }
        }
    }
}
