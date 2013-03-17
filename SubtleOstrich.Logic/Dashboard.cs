using System.Collections.Generic;

namespace SubtleOstrich.Logic
{
    public class Dashboard
    {
        public IEnumerable<Status> Activities { get; set; }

        public int Total { get; set; }

        public string Title { get; set; }
    }
}