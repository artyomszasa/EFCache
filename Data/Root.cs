using System.Collections.Generic;

namespace EFCache.Data
{
    public class Root
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public ICollection<Word> Words { get; set; }
    }
}