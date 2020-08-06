using System;
using System.Globalization;
using System.Text;
using UserDataFaker.Library;

namespace UserDataFaker.ConsoleUI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 3)
            {
                Console.WriteLine("Wrong number of arguments");
                return;
            }

            string locale = args[0];
            if (locale != "en_US" && locale != "ru_RU" && locale != "be_BY")
            {
                Console.WriteLine("Unsupported locale");
                return;
            }

            long generatedRecordsNumber;
            if (!long.TryParse(args[1], out generatedRecordsNumber) || generatedRecordsNumber < 1)
            {
                Console.WriteLine("Wrong generated records number");
                return;
            }

            double mistakesNumber = 0;
            if (args.Length >= 3)
            {
                if (!double.TryParse(args[2], NumberStyles.Any, CultureInfo.InvariantCulture, out mistakesNumber) || mistakesNumber < 0)
                {
                    Console.WriteLine("Wrong mistakes number");
                    return;
                }
            }

            CustomFaker faker = new CustomFaker(locale, mistakesNumber);

            Console.OutputEncoding = Encoding.UTF8;

            for (int i = 0; i < generatedRecordsNumber; i++)
            {
                Console.WriteLine(faker.GetRandomRecordWithMistakes());
            }
        }
    }
}
