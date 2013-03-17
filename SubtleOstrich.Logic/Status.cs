namespace SubtleOstrich.Logic
{
    public class Status
    {
        public string Name { get; private set; }

        public int Value { get; private set; }

        public Status(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}