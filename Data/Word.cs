namespace EFCache.Data
{
    public class Word
    {
        public int Id { get; set; }

        public int RootId { get; set; }

        public Root Root { get; set; }

        public string Data { get; set; }
    }
}