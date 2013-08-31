using System.Collections.Generic;

namespace SubtleOstrich.Logic
{
    public class Dashboard
    {
        public IEnumerable<Status> Activities { get; set; }

        public decimal Total { get; set; }

        public string Title { get; set; }
    }
}