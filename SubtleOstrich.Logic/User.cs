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

        public User()
            :this(new UserRepository())
        {
            
        }

        public User(IUserRepository repo)
        {
            _repo = repo;
            Activities = new List<Activity>();
        }

        public void AddActivity(Activity activity)
        {
            Activities.Add(activity);
        }

        public IList<Activity> Activities { get; set; }

        public int GetMonthTotal(int month)
        {
            return Activities.Sum(activity => activity.GetMonthlyTotal(month));
        }

        public void AddRecord(string activityName, Record record)
        {
            var activity = Activities.FirstOrDefault(act => act.Name.Equals(activityName, StringComparison.InvariantCultureIgnoreCase));
            if (activity == null)
            {
                activity = new Activity(activityName);
                Activities.Add(activity);
            }

            activity.AddRecord(record);
        }

        public int GetYearTotal(int year)
        {
            return Activities.Sum(activity => activity.GetYearlyTotal(year));
        }

        public void Save()
        {
            _repo.Save(this);
        }

        public string Id { get; set; }
    }
}
