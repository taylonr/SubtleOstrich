using System;

namespace SubtleOstrich.Logic
{
    public class Occurrence
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }

        public Occurrence(string id, string name, DateTime date, string note)
        {
            Id = id;
            Name = name;
            Date = date;
            Note = note;
        }
    }
}