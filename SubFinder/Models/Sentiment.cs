namespace SubFinder.Models
{
    public class Sentiment
    {
        public int SentimentId { get; set; }
        public int Polarity { get; set; } // -1 for negative, 0 for neutral, 1 for positive
        public string Unit { get; set; }
        public double Strength { get; set; } // Objectivity score
        public int ListId { get; set; }
    }

    public class CustomRule
    {
        public int CustomRuleId { get; set; }
        public string Unit { get; set; }
        public int ExecutionRule { get; set; } // 0 excludes this unit, 1 excludes the unit and the sentence, 2 executes the execution parameters
        public int ExecutionParameters { get; set; } // for this build, we're using this as ObjScore (strength)
    }
}
