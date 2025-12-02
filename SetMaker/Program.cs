using System;
using System.Collections.Generic;
using Bogus;

namespace TestKniznice
{
    public enum SetAction
    {
        KEEP,
        REMOVE,
        ADD
    }

    public static class Program
    {
        const int MAX_RESULT_SET_SIZE = 34;
        const int MIN_RESULT_SET_SIZE = 17;

        public static void Main()
        {
            var faker = new Faker();

            // Pocet slov v Result Sete
            int targetCount = faker.Random.Int(MIN_RESULT_SET_SIZE, MAX_RESULT_SET_SIZE);

            // HashSet bez duplikátov
            var resultSet = new HashSet<string>(StringComparer.Ordinal);

            int attempts = 0;
            const int maxAttempts = 1000;

            while (resultSet.Count < targetCount)
            {
                if (++attempts > maxAttempts)
                    throw new InvalidOperationException($"Failed to generate {targetCount} unique values after {maxAttempts} attempts.");

                int length = faker.Random.Int(8, 24);
                string value = faker.Random.Word();
                resultSet.Add(value);
            }

            Console.WriteLine($"Created set with {resultSet.Count} unique items:");
            foreach (var item in resultSet)
            {
                Console.WriteLine(item);
            }

            var rightSet = new HashSet<string>(resultSet, StringComparer.Ordinal);
            var leftSet = new HashSet<string>(resultSet, StringComparer.Ordinal);
            var baseSet = new HashSet<string>(resultSet, StringComparer.Ordinal);


        }
    }
}