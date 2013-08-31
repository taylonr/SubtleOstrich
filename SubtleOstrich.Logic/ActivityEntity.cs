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

        public decimal GetMonthlyTotal(int month)
        {
            if(Hours > 0)
            {
                decimal total = 0;
                foreach (var r in Records.Where(record => record.Date.Month == month && record.Date.Year == DateTime.Now.Year))
                {
                    if (r.Time.HasValue)
                        total += r.Time.Value;
                    else
                        total += Hours;
                }

                return total;
            }
            
            return Records.Count(record => record.Date.Month == month && record.Date.Year == DateTime.Now.Year);
        }

        public decimal GetYearlyTotal(int year)
        {
            if(Hours > 0)
            {
                decimal total = 0;
                foreach (var r in Records.Where(record => record.Date.Year == year))
                {
                    if (r.Time.HasValue)
                        total += r.Time.Value;
                    else
                        total += Hours;
                }

                return total;
            }

            return Records.Count(record => record.Date.Year == year);
        }

        public IEnumerable<Record> GetNotesForMonth(int month)
        {
            return Records.Where(record => !string.IsNullOrWhiteSpace(record.Note)).ToList();
        }
    }
}
