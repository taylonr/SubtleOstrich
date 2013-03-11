using System;
using MongoRepository;

namespace SubtleOstrich.Logic
{
    public class Record : IEntity
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string Note { get; set; }

        public Record()
        {
            if(Id == null)
                Id = DateTime.Now.ToString("MMddyyyyhh:mm:ss.fff");
        }

        public Record(DateTime date, string note = "")
            :this()
        {
            Date = date;
            Note = note;
        }
    }
}