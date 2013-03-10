using System;
using MongoRepository;

namespace SubtleOstrich.Logic
{
    public class Record : Entity
    {
        public DateTime Date { get; set; }

        public string Note { get; set; }

        public Record()
        {
            
        }

        public Record(DateTime date, string note = "")
        {
            Date = date;
            Note = note;
        }
    }
}