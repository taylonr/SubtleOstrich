namespace SubtleOstrich.Logic
{
    public class Status
    {
        public string Name { get; private set; }

        public decimal Value { get; private set; }

        public Status(string name, decimal value)
        {
            Name = name;
            Value = value;
        }
    }
}