using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoRepository;

namespace SubtleOstrich.Logic
{
    public class Activity : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Hours { get; set; }

        public IList<Record> Records { get; set; }

        public Activity()
        {
            Records = new List<Record>();
        }

        public Activity(string name)
            : this()
        {
            Id = name;
            Name = name;
            
        }

        public void AddRecord(Record record)
        {
            Records.Add(record);
        }

        public int GetMonthlyTotal(int month)
        {
            return Records.Count(record => record.Date.Month == month && record.Date.Year == DateTime.Now.Year);
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
