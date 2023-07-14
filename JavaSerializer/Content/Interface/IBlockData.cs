namespace JavaSerializer.Content.Interface
{
    public interface IBlockData : IContent
    {
        byte[]? Data { get; set; }
    }
}