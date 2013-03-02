using System;

namespace SubtleOstrich.Logic
{
    public class Record
    {
        public DateTime Date { get; private set; }

        public string Note { get; private set; }

        public Record(DateTime date, string note = "")
        {
            Date = date;
            Note = note;
        }
    }
}