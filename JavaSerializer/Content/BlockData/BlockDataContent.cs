using JavaSerializer.Content.Interface;

namespace JavaSerializer.Content.BlockData
{
    public class BlockDataContent : IBlockData
    {
        public BlockDataContent(TokenType contentType)
        {
            Header = contentType;
        }

        public TokenType Header { get; }
        public int Size { get; set; }
        public byte[]? Data { get; set; }
    }
}