namespace UserDataFaker.Library
{
    public class Record
    {
        public string FullName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public override string ToString()
        {
            return $"{FullName}; {Address}; {PhoneNumber}";
        }
    }
}
