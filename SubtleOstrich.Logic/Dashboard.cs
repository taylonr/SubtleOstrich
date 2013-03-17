using System.Collections.Generic;

namespace SubtleOstrich.Logic
{
    public class Dashboard
    {
        public IEnumerable<Status> Activities { get; set; }

        public int Total { get; set; }
    }

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