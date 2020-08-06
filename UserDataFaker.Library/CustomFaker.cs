using System;

namespace UserDataFaker.Library
{
    public class CustomFaker
    {
        private readonly string locale;

        private readonly double mistakesNumber;

        private readonly Bogus.DataSets.Name nameDataSet;

        private readonly Bogus.DataSets.Address addressDataSet;

        private readonly Bogus.DataSets.PhoneNumbers phoneNumbersDataSet;

        private readonly Bogus.DataSets.Lorem loremDataSet;

        private readonly Random random;
        
        public CustomFaker(string locale, double mistakesNumber)
        {
            switch(locale)
            {
                case "en_US":
                    this.locale = "en_US";
                    break;
                case "ru_RU":
                    this.locale = "ru";
                    break;
                case "be_BY":
                    this.locale = "be";
                    break;
                default:
                    throw new ArgumentException($"Unsupported locale: {locale}");
            }

            this.mistakesNumber = mistakesNumber;
            nameDataSet = new Bogus.DataSets.Name(this.locale);
            addressDataSet = new Bogus.DataSets.Address(this.locale);
            phoneNumbersDataSet = new Bogus.DataSets.PhoneNumbers(this.locale);
            loremDataSet = new Bogus.DataSets.Lorem(this.locale);
            random = new Random();
        }

        public Record GetRandomRecord()
        {
            return new Record
            {
                FullName = nameDataSet.FullNameWithMiddle(),
                Address = addressDataSet.FullAddressFormatted(),
                PhoneNumber = phoneNumbersDataSet.PhoneNumberFormat()
            };
        }

        public Record GetRandomRecordWithMistakes()
        {
            Record record = GetRandomRecord();

            if (mistakesNumber == 0)
            {
                return record;
            }
            
            int mistakesNumberInRecord = GetMistakesNumberInRecord();

            Func<string, string> mistakeFunc;

            RecordAttribute recordAttribute;

            for (int i = 0; i < mistakesNumberInRecord; i++)
            {
                mistakeFunc = GetMistakeFunction();

                recordAttribute = RandomEnumValue<RecordAttribute>();

                switch (recordAttribute)
                {
                    case RecordAttribute.FullName:
                        record.FullName = mistakeFunc(record.FullName);
                        break;
                    case RecordAttribute.Address:
                        record.Address = mistakeFunc(record.Address);
                        break;
                    case RecordAttribute.PhoneNumber:
                        record.PhoneNumber = mistakeFunc(record.PhoneNumber);
                        break;
                    default:
                        throw new ArgumentException("Unsupported record attribute");
                }
            }

            return record;
        }

        private int GetMistakesNumberInRecord()
        {
            double wholePart = Math.Truncate(mistakesNumber);
            double decimalPart = mistakesNumber - wholePart;

            return (int)wholePart + (random.NextDouble() < decimalPart ? 1 : 0);
        }

        private T RandomEnumValue<T>()
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(random.Next(values.Length));
        }

        private Func<string, string> GetMistakeFunction()
        {
            MistakeType mistakeType = RandomEnumValue<MistakeType>();

            switch (mistakeType)
            {
                case MistakeType.AddRandomCharacter:
                    return AddRandomCharacter;
                case MistakeType.DeleteRandomCharacter:
                    return DeleteRandomCharacter;
                case MistakeType.SwapAdjacentCharaters:
                    return SwapAdjacentCharacters;
                default:
                    throw new ArgumentException("Unsupported mistake type");
            }
        }

        private string DeleteRandomCharacter(string str)
        {
            if (str.Length == 0)
            {
                return str;
            }

            int randomCharIndex = random.Next(0, str.Length);

            return str.Remove(randomCharIndex, 1);
        }

        private string AddRandomCharacter(string str)
        {
            int randomCharIndex = random.Next(0, str.Length);

            string randomAlphaNumeric = loremDataSet.AlphaNumeric();

            return str.Insert(randomCharIndex, randomAlphaNumeric);
        }

        private string SwapAdjacentCharacters(string str)
        {
            if (str.Length < 2)
            {
                return str;
            }
            
            int randomCharIndex = random.Next(0, str.Length - 1);
            string valueToSwap = str[randomCharIndex].ToString();

            return str.Remove(randomCharIndex, 1).Insert(randomCharIndex + 1, valueToSwap);
        }
    }
}
