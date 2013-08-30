using System;

namespace SubtleOstrich.Logic
{
    public class Occurrence
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public decimal? Hours { get; set; }

        public Occurrence()
        {
            
        }

        public Occurrence(string id, string name, DateTime date, string note, decimal? hours = null)
        {
            Id = id;
            Name = name;
            Date = date;
            Note = note;
            Hours = hours;
        }
    }
}