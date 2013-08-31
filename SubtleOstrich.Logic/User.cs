using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoRepository;

namespace SubtleOstrich.Logic
{
    public class User : IEntity
    {
        private readonly IUserRepository _repo;

        public string Id { get; set; }

        public string Name { get; set; }

        public User(string uid, string source)
            :this(uid, null, source)
        {
        }

        public User(string id, string name, string source)
            :this(id, name, source, new UserRepository())
        {
            Activities = _repo.GetActivities(Id);
            Name = _repo.GetName(Id);
        }

        public User(string uid, string name, string source, IUserRepository repo)
        {
            _repo = repo;
            Activities = new List<Activity>();
            Id = string.Format("{0}:{1}", source, uid);
            Name = name;
        }

        public static User CreateUser(string id, string name, string source)
        {
            var u = new User(id, name, source, new UserRepository());
            u.Save();
            return u;
        }

        public void AddActivity(Activity activity)
        {
            Activities.Add(activity);
        }

        public IList<Activity> Activities { get; set; }

        public decimal GetMonthTotal(int month)
        {
            return Activities.Sum(activity => activity.GetMonthlyTotal(month));
        }

        public string AddRecord(string activityName, Record record)
        {
            var activity = Activities.FirstOrDefault(act => act.Name.Equals(activityName, StringComparison.InvariantCultureIgnoreCase));
            if (activity == null)
            {
                activity = new Activity(activityName) {Hours = record.Time ?? 0};
                Activities.Add(activity);
            }

            activity.AddRecord(record);

            Save();

            return record.Id;
        }

        public decimal GetYearTotal(int year)
        {
            return Activities.Sum(activity => activity.GetYearlyTotal(year));
        }

        public void Save()
        {
            _repo.Save(this);
        }



        public IEnumerable<Occurrence> GetActivities(DateTime date)
        {
            var activities = _repo.GetActivities(Id, date);

            var occurrences = new List<Occurrence>();
            foreach(var a in activities)
            {
                var time = a.Hours;
                if(time > 0)
                    occurrences.AddRange(a.Records.Where(x => x.Date.ToLocalTime().ToShortDateString() == date.ToShortDateString()).Select(r => new Occurrence(r.Id, a.Name, r.Date, r.Note, r.Time ?? time)));
                else
                    occurrences.AddRange(a.Records.Where(x => x.Date.ToLocalTime().ToShortDateString() == date.ToShortDateString()).Select(r => new Occurrence(r.Id, a.Name, r.Date, r.Note, r.Time)));
            }
                

            return occurrences;
        }

        public void DeleteRecord(string id)
        {
            var activity = Activities.FirstOrDefault(x => x.Records.Any(record => record.Id == id));
            activity.Records.Remove(activity.Records.FirstOrDefault(record => record.Id == id));
            Save();
        }

        public Dashboard GetMonthDashboard()
        {
            var month = DateTime.Now.Month;
            return new Dashboard
                {
                    Activities = Activities.OrderByDescending(act => act.GetMonthlyTotal(month)).Select(x => new Status(x.Name, x.GetMonthlyTotal(month))).Take(4),
                    Total = GetMonthTotal(month),
                    Title = DateTime.Now.ToString("MMMM")
                };
        }

        public Dashboard GetYearDashboard()
        {
            var year = DateTime.Now.Year;
            return new Dashboard
                {
                    Activities = Activities.OrderByDescending(act => act.GetYearlyTotal(year)).Select(x => new Status(x.Name, x.GetYearlyTotal(year))).Take(4),
                    Total = GetYearTotal(year),
                    Title = year.ToString()
                };
        }

        public IEnumerable<CalendarEvent> GetCalendarEvents(DateTime start, DateTime end)
        {
            var events = new List<CalendarEvent>();
            foreach (var a in Activities)
                events.AddRange(a.Records.Where(x => start <= x.Date || x.Date >= end)
                    .Select(r => new CalendarEvent {title = a.Name, start = r.Date.ToUnixTimestamp()}));

            return events;
        }

        public Dashboard GetYearReport(int year)
        {
            return new Dashboard
            {
                Activities = Activities.OrderByDescending(act => act.GetYearlyTotal(year)).Select(x => new Status(x.Name, x.GetYearlyTotal(year))).ToList(),
                Total = GetYearTotal(year),
                Title = year.ToString()
            };
        }

        public Dashboard GetMonthReport(DateTime now)
        {
            return new Dashboard
            {
                Activities = Activities.OrderByDescending(act => act.GetMonthlyTotal(now.Month)).Select(x => new Status(x.Name, x.GetMonthlyTotal(now.Month))).ToList(),
                Total = GetMonthTotal(now.Month),
                Title = now.ToLongDateString()
            };
        }
    }
}
