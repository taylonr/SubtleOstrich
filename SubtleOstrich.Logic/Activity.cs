using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtleOstrich.Logic
{
    public class Activity
    {
        public string Name { get; set; }

        public IList<Record> Records { get; set; }

        public Activity()
        {
        }

        public Activity(string name)
        {
            Name = name;
            Records = new List<Record>();
        }

        public void AddRecord(Record record)
        {
            Records.Add(record);
        }

        public int GetMonthlyTotal(int month)
        {
            return Records.Count(record => record.Date.Month == month);
        }

        public int GetYearlyTotal(int year)
        {
            return Records.Count(record => record.Date.Year == year);
        }

        public IEnumerable<Record> GetNotesForMonth(int month)
        {
            return Records.Where(record => !string.IsNullOrWhiteSpace(record.Note)).ToList();
        }
    }
}
