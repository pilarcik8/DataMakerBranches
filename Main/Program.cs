using Bogus;
using Bogus.Bson;
using Bogus.DataSets;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Collections.Specialized.BitVector32;


namespace TestKniznice
{
    public enum AtributeAction
    {
        KEEP,
        CHANGE,
        REMOVE,
        ADD
    }

    public static class Program
    {
        // Nastavenia generovania
        const int ITERATIONS = 10;
        const bool ALLOW_CHANGE = true;
        const bool ALLOW_REMOVE = true;
        const bool ALLOW_ADD = true;

        public static void Main()
        {
            // Generovanie testovacich dat
            var iterations = ITERATIONS - 1;
            for (int j = 0; j < iterations; j++) {
            // Vytvorenie vyslednej osoby
            var faker = new Faker("en");
            Person resultPerson = CreateFakePerson(faker);
            int baseAtributeCount = typeof(Person).GetProperties().Length; //null nepocita

            // Tvorba 2 branchov a ich predka
            Person leftPerson = resultPerson.Clone();
            Person rightPerson = resultPerson.Clone();
            Person basePeson = resultPerson.Clone();

            // Pre vasciu diferenciaciu r a l branchov pocas generovania
            double leftKeepProbability = Random.Shared.NextDouble() * 0.6 + 0.2; // [0.2, 0.8]
            double rightKeepProbability = 1.0 - leftKeepProbability;
            Console.WriteLine($"Left KEEP probability: {leftKeepProbability:P0}, Right KEEP probability: {rightKeepProbability:P0}");

            for (int i = 0; i < baseAtributeCount; i++)
            {
                // Generovanie akcii pre pravy a lavy branch
                AtributeAction actionR, actionL;
                bool leftIsKeep = Random.Shared.NextDouble() < leftKeepProbability;

                if (leftIsKeep)
                {
                    actionL = AtributeAction.KEEP;
                    actionR = GetAtributeAction();
                }
                else
                {
                    actionL = GetAtributeAction();
                    actionR = AtributeAction.KEEP;

                }

                if (actionR == AtributeAction.KEEP && actionL == AtributeAction.KEEP)
                {
                    Console.WriteLine($"\n{i + 1}. R + L + B action:");
                    // nezalezi ktory branch sa vyberie, lebo oba maju KEEP
                    ExecuteSameAction(rightPerson, basePeson, i, actionR, faker);
                }
                else if (actionR == AtributeAction.KEEP)
                {
                    Console.WriteLine($"\n{i + 1}. L + B action:");
                    ExecuteSameAction(leftPerson, basePeson, i, actionL, faker);
                }
                else if (actionL == AtributeAction.KEEP)
                {
                    Console.WriteLine($"\n{i + 1}. R + B action:");
                    ExecuteSameAction(rightPerson, basePeson, i, actionR, faker);
                }
            }
            Console.WriteLine();
            ExportPerson(resultPerson, "res", j);
            ExportPerson(rightPerson, "right", j);
            ExportPerson(leftPerson, "left", j);
            ExportPerson(basePeson, "base", j);
            Console.WriteLine("-----------------------------------------------------\n");
            }
        }

        private static void ExecuteSameAction(Person branchPerson, Person basePerson, int i, AtributeAction action, Faker faker)
        {
            if (action == AtributeAction.KEEP)
            {
                Console.WriteLine($"    Kept attribute: '{branchPerson.GetAttributeName(i)}'");
                return;

            }
            else if (action == AtributeAction.CHANGE)
            {
                string change = branchPerson.ChangeAttribute(i, faker);
                basePerson.SetAttribute(i, change);
            }
            else if (action == AtributeAction.REMOVE)
            {
                Console.WriteLine($"    Removed attribute: '{branchPerson.GetAttributeName(i)}'");
                branchPerson.RemoveAtribute(i);
                basePerson.RemoveAtribute(i);
            }

            else if (action == AtributeAction.ADD)
            {
                string valueAndNameOfNewAttribute = branchPerson.AddAttribute(i, faker);
                string[] parts = valueAndNameOfNewAttribute.Split('|');
                string valueOfNewAttribute = parts[0];
                string nameOfNewAttribute = parts[1];
                Console.WriteLine($"    Added new attribute after '{branchPerson.GetAttributeName(i)}': named '{nameOfNewAttribute}' with value '{valueOfNewAttribute}'");
                basePerson.AddAttribute(i, faker, valueOfNewAttribute);
            }
        }

        public static AtributeAction GetAtributeAction()
        {
            int randomValue = new Random().Next(3);

            switch(randomValue)
            {
                case 0:
                    return AtributeAction.KEEP;

                case 1:
                    if (ALLOW_CHANGE)
                        return AtributeAction.CHANGE;
                    else
                        return GetAtributeAction();

                case 2:
                    if (ALLOW_REMOVE)
                        return AtributeAction.REMOVE;
                    else
                        return GetAtributeAction();

                case 3:
                    if (ALLOW_ADD)
                        return AtributeAction.ADD;
                    else
                        return GetAtributeAction();
            }

            return (AtributeAction)new Random().Next(4);
        }


        private static Person CreateFakePerson(Faker faker)
        {
            var personFaker = new Faker<Person>("en")
                .RuleFor(p => p.Title, f => f.Name.Prefix())
                .RuleFor(p => p.FirstName, f => f.Name.FirstName())
                .RuleFor(p => p.LastName, f => f.Name.LastName())
                .RuleFor(p => p.Email, f => f.Internet.Email())
                .RuleFor(p => p.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(p => p.Gender, f => f.PickRandom(new[] { "Male", "Female", "Other" }))
                .RuleFor(p => p.Age, f => f.Random.Int(18, 80))
                .RuleFor(p => p.Company, f => f.Company.CompanyName())
                .RuleFor(p => p.JobTitle, f => f.Name.JobTitle())
                .RuleFor(p => p.CreditCardNumber, f => f.Finance.CreditCardNumber())
                .RuleFor(p => p.Street, f => f.Address.StreetName())
                .RuleFor(p => p.StreetNumber, f => f.Address.SecondaryAddress())
                .RuleFor(p => p.City, f => f.Address.City())
                .RuleFor(p => p.County, f => f.Address.County())
                .RuleFor(p => p.State, f => f.Address.State())
                .RuleFor(p => p.ZipCode, f => f.Address.ZipCode())
                .RuleFor(p => p.Country, f => f.Address.Country());

            return personFaker.Generate();
        }

        private static void ExportPerson(Person person, string fileName, int iteration)
        {
            if (person == null)
            {
                Console.WriteLine("Person je null – export sa nevykoná.");
                return;
            }

            try
            {
                // Relatívna cesta ku koreňu projektu
                string projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));
                string outputDir = Path.Combine(projectDir, "createdFiles");

                // Vytvorenie priečinku, ak neexistuje
                Directory.CreateDirectory(outputDir);

                string xmlPath = Path.Combine(outputDir, $"{fileName}{iteration}.xml");
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Person));
                using (var writer = new StreamWriter(xmlPath))
                {
                    xmlSerializer.Serialize(writer, person);
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