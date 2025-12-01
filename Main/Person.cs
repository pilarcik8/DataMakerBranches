using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using Bogus;

namespace TestKniznice
{
    public class Person
    {
        public string? Title { get; set; } //0
        public string? FirstName { get; set; } //1
        public string? LastName { get; set; } //2

        public string? Email { get; set; } //3
        public string? Phone { get; set; } //4

        public string? Gender { get; set; }  //5
        public int? Age { get; set; } //6
        public string? Company { get; set; } //7
        public string? JobTitle { get; set; } //8

        public string? CreditCardNumber { get; set; } //9
        public string? Street { get; set; } //10
        public string? StreetNumber { get; set; } //11
        public string? City { get; set; } //12
        public string? County { get; set; } //13
        public string? State { get; set; } //14
        public string? ZipCode { get; set; } //15
        public string? Country { get; set; } //16

        public Person()
        {
            Title = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            Gender = string.Empty;
            Age = 0;
            Company = string.Empty;
            JobTitle = string.Empty;
            CreditCardNumber = string.Empty;
            Street = string.Empty;
            StreetNumber = string.Empty;
            City = string.Empty;
            County = string.Empty;
            State = string.Empty;
            ZipCode = string.Empty;
            Country = string.Empty;
        }

        public string ChangeAttribute(int i, Faker faker)
        {
            if (faker == null) throw new ArgumentNullException(nameof(faker));

            var old = GetAttribute(i);
            string newValue;

            switch (i)
            {
                case 0:
                    {
                        newValue = faker.Name.Prefix();
                        while (newValue == old)
                            newValue = faker.Name.Prefix();
                        break;
                    }

                case 1:
                    {
                        newValue = faker.Name.FirstName();
                        while (newValue == old)
                            newValue = faker.Name.FirstName();
                        break;
                    }

                case 2:
                    {
                        newValue = faker.Name.LastName();
                        while (newValue == old)
                            newValue = faker.Name.LastName();
                        break;
                    }

                case 3:
                    {
                        newValue = faker.Internet.Email();
                        while (string.Equals(newValue, old, StringComparison.OrdinalIgnoreCase))
                            newValue = faker.Internet.Email();
                        break;
                    }

                case 4:
                    {
                        newValue = faker.Phone.PhoneNumber();
                        while (newValue == old)
                            newValue = faker.Phone.PhoneNumber();
                        break;
                    }

                case 5:
                    {
                        newValue = faker.PickRandom(new[] { "Male", "Female", "Other" });
                        while (newValue == old)
                            newValue = faker.PickRandom(new[] { "Male", "Female", "Other" });
                        break;
                    }

                case 6:
                    {
                        int newAge = faker.Random.Int(18, 80);
                        while (newAge.ToString() == old)
                            newAge = faker.Random.Int(18, 80);

                        newValue = newAge.ToString();
                        break;
                    }

                case 7:
                    {
                        newValue = faker.Company.CompanyName();
                        while (newValue == old)
                            newValue = faker.Company.CompanyName();
                        break;
                    }

                case 8:
                    {
                        newValue = faker.Name.JobTitle();
                        while (newValue == old)
                            newValue = faker.Name.JobTitle();
                        break;
                    }

                case 9:
                    {
                        newValue = faker.Finance.CreditCardNumber();
                        while (newValue == old)
                            newValue = faker.Finance.CreditCardNumber();
                        break;
                    }

                case 10:
                    {
                        newValue = faker.Address.StreetName();
                        while (newValue == old)
                            newValue = faker.Address.StreetName();
                        break;
                    }

                case 11:
                    {
                        newValue = faker.Address.SecondaryAddress();
                        while (newValue == old)
                            newValue = faker.Address.SecondaryAddress();
                        break;
                    }

                case 12:
                    {
                        newValue = faker.Address.City();
                        while (newValue == old)
                            newValue = faker.Address.City();
                        break;
                    }

                case 13:
                    {
                        newValue = faker.Address.County();
                        while (newValue == old)
                            newValue = faker.Address.County();
                        break;
                    }

                case 14:
                    {
                        newValue = faker.Address.State();
                        while (newValue == old)
                            newValue = faker.Address.State();
                        break;
                    }

                case 15:
                    {
                        newValue = faker.Address.ZipCode();
                        while (newValue == old)
                            newValue = faker.Address.ZipCode();
                        break;
                    }

                case 16:
                    {
                        newValue = faker.Address.Country();
                        while (newValue == old)
                            newValue = faker.Address.Country();
                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(i), i, "Invalid attribute index");
            }
            SetAttribute(i, newValue);

            Console.WriteLine($"    Changed attribute: '{GetAttributeName(i)}' from '{old}' to '{newValue}'");
            return newValue;
        }

        public string? GetAttribute(int i)
        {
            switch (i)
            {
                case 0: return Title;
                case 1: return FirstName;
                case 2: return LastName;
                case 3: return Email;
                case 4: return Phone;
                case 5: return Gender;
                case 6: return Age?.ToString();
                case 7: return Company;
                case 8: return JobTitle;
                case 9: return CreditCardNumber;
                case 10: return Street;
                case 11: return StreetNumber;
                case 12: return City;
                case 13: return County;
                case 14: return State;
                case 15: return ZipCode;
                case 16: return Country;
                default:
                    throw new ArgumentOutOfRangeException(nameof(i), i, "Invalid attribute index");
            }
        }

        internal void RemoveAtribute(int i)
        {
            SetAttribute(i, null);
        }


        internal void SetAttribute(int i, string? value)
        {
            var old = GetAttribute(i);
            var name = GetAttributeName(i);

            switch (i)
            {
                case 0:
                    {
                        Title = value;
                        break;
                    }
                case 1:
                    {
                        FirstName = value;
                        break;
                    }
                case 2:
                    {
                        LastName = value;
                        break;
                    }
                case 3:
                    {
                        Email = value;
                        break;
                    }
                case 4:
                    {
                        Phone = value;
                        break;
                    }
                case 5:
                    {
                        Gender = value;
                        break;
                    }
                case 6:
                    {
                        if (value == null)
                        {
                            Age = null;
                            break;
                        }

                        if (!int.TryParse(value, out var parsedAge))
                            throw new FormatException($"Cannot parse Age from '{value}'.");
                        Age = parsedAge;
                        break;
                    }
                case 7:
                    {
                        Company = value;
                        break;
                    }
                case 8:
                    {
                        JobTitle = value;
                        break;
                    }
                case 9:
                    {
                        CreditCardNumber = value;
                        break;
                    }
                case 10:
                    {
                        Street = value;
                        break;
                    }
                case 11:
                    {
                        StreetNumber = value;
                        break;
                    }
                case 12:
                    {
                        City = value;
                        break;
                    }
                case 13:
                    {
                        County = value;
                        break;
                    }
                case 14:
                    {
                        State = value;
                        break;
                    }
                case 15:
                    {
                        ZipCode = value;
                        break;
                    }
                case 16:
                    {
                        Country = value;
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(i), i, "Invalid attribute index");
            }

            //Console.WriteLine($"'{name}' set: '{old}' -> '{value}'");

        }

        public string GetAttributeName(int i)
        {
            switch (i)
            {
                case 0: return nameof(Title);
                case 1: return nameof(FirstName);
                case 2: return nameof(LastName);
                case 3: return nameof(Email);
                case 4: return nameof(Phone);
                case 5: return nameof(Gender);
                case 6: return nameof(Age);
                case 7: return nameof(Company);
                case 8: return nameof(JobTitle);
                case 9: return nameof(CreditCardNumber);
                case 10: return nameof(Street);
                case 11: return nameof(StreetNumber);
                case 12: return nameof(City);
                case 13: return nameof(County);
                case 14: return nameof(State);
                case 15: return nameof(ZipCode);
                case 16: return nameof(Country);
                default:
                    throw new ArgumentOutOfRangeException(nameof(i), i, "Invalid attribute index");
            }
        }

        public Person Clone()
        {
            return new Person
            {
                Title = this.Title,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                Phone = this.Phone,
                Gender = this.Gender,
                Age = this.Age,
                Company = this.Company,
                JobTitle = this.JobTitle,
                CreditCardNumber = this.CreditCardNumber,
                Street = this.Street,
                StreetNumber = this.StreetNumber,
                City = this.City,
                County = this.County,
                State = this.State,
                ZipCode = this.ZipCode,
                Country = this.Country
            };
        }
    }
}