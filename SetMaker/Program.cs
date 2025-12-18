using Bogus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

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
        const int ITERATIONS = 1;

        public static void Main()
        {
            var faker = new Faker();

            int targetCount = faker.Random.Int(MIN_RESULT_SET_SIZE, MAX_RESULT_SET_SIZE);

            for (int i = 0; i < ITERATIONS; i++)
            {

                var resultSet = new HashSet<string>(StringComparer.Ordinal);

                while (resultSet.Count < targetCount)
                {
                    string value = faker.Random.Word();
                    if (!resultSet.Contains(value))
                    {
                        resultSet.Add(value);
                    }
                }

                var rightSet = new HashSet<string>(resultSet, StringComparer.Ordinal);
                var leftSet = new HashSet<string>(resultSet, StringComparer.Ordinal);
                var baseSet = new HashSet<string>(resultSet, StringComparer.Ordinal);

                double leftKeepProbability = Random.Shared.NextDouble() * 0.6 + 0.2; // [0.2, 0.8]
                double rightKeepProbability = 1.0 - leftKeepProbability;
                Console.WriteLine($"Left KEEP probability: {leftKeepProbability:P0}, Right KEEP probability: {rightKeepProbability:P0}");

                foreach (string item in resultSet)
                {
                    SetAction leftAct, rightAct;
                    if (Random.Shared.NextDouble() > leftKeepProbability)
                    {
                        leftAct = SetAction.KEEP;
                        rightAct = GetAction();
                    }
                    else
                    {
                        leftAct = GetAction();
                        rightAct = SetAction.KEEP;
                    }

                    if (leftAct == SetAction.KEEP && rightAct == SetAction.KEEP)
                    {
                        Console.WriteLine("L, R, B:");
                        continue;
                    }
                    else if (leftAct == SetAction.KEEP)
                    {
                        Console.WriteLine("R, B:");
                        ExecuteAction(rightSet, baseSet, item, rightAct, faker);
                    }
                    else if (rightAct == SetAction.KEEP)
                    {
                        Console.WriteLine("L, B:");
                        ExecuteAction(leftSet, baseSet, item, leftAct, faker);
                    }
                }

                // build iteration directory and export files there
                string iterDir = Path.Combine("createdFiles", (i).ToString());
                ExportSet(leftSet, "left", i);
                ExportSet(rightSet, "right", i);
                ExportSet(baseSet, "base", i);
                ExportSet(resultSet, "result", i);

            }

        }

        private static void ExecuteAction(HashSet<string> branchSet, HashSet<string> baseSet, string item, SetAction action, Faker faker)
        {
            if (action == SetAction.KEEP)
            {
                Console.WriteLine($"Keeping item: {item}");
                return;
            }
            else if (action == SetAction.REMOVE)
            {
                Console.WriteLine($"Removing item: {item}");
                baseSet.Remove(item);
                branchSet.Remove(item);
            }
            else if (action == SetAction.ADD)
            {
                string newItem = faker.Random.Word();
                while (branchSet.Contains(newItem))
                {
                    newItem = faker.Random.Word();
                }
                baseSet.Add(newItem);
                branchSet.Add(newItem);
            }
        }

        private static SetAction GetAction()
        {
            int randomValue = new Random().Next(3);
            switch (randomValue)
            {
                case 0:
                    return SetAction.KEEP;
                case 1:
                    return SetAction.REMOVE;
                default:
                    return SetAction.ADD;
            }
        }

        private static void ExportSet(HashSet<string> set, string fileName, int iteration)
        {
            if (set == null) throw new ArgumentNullException(nameof(set));

            try
            {
                string projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));
                string outputDir = Path.Combine(projectDir, "createdFiles", iteration.ToString());
                Directory.CreateDirectory(outputDir);

                string xmlPath = Path.Combine(outputDir, $"{fileName}{iteration}.xml");
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(HashSet<string>));
                using (var writer = new StreamWriter(xmlPath))
                {
                    xmlSerializer.Serialize(writer, set);
                }
                Console.WriteLine($"XML uložený do: {xmlPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba pri exporte: {ex.Message}");
            }
        }
    }
}