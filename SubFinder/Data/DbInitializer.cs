using System;
using System.Linq;
using SubFinder.Models;

namespace SubFinder.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SubFinderContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Sentiment.Any())
            {
                return;   // DB has been seeded
            }

            var sentiments = new Sentiment[]
            {
            new Sentiment{ListId = 1, Polarity = 1, Strength = 1.0, Unit = "able"}
            };
            foreach (Sentiment s in sentiments)
            {
                context.Sentiment.Add(s);
            }
            context.SaveChanges();

        }
    }
}
