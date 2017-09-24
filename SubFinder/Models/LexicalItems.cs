using System.Collections.Generic;

namespace SubFinder.Models
{
    public class LexicalItems
    {
        public class Word
        {
            public int WordID { get; set; }
            public string Unit { get; set; }
            public List<Sentiment> Values { get; set; }
        }

        public class Sentence
        {
            public int SentenceID { get; set; }
            public string SentenceUnit { get; set; }
            public List<Sentiment> Values { get; set; }
        }
     
    }

}
