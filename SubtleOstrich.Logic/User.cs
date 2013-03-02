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

        public Guid Identifier { get; set; }

        public string Name { get; set; }

        public User(string email, string name, string source)
            :this(email, name, source, new UserRepository())
        {
            
        }

        public User(string uid, string name, string source, IUserRepository repo)
        {
            _repo = repo;
            Activities = new List<Activity>();
            Id = string.Format("{0}:{1}", source, uid);
            Name = name;
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
            if(Identifier == Guid.Empty)
                Identifier = Guid.NewGuid();

            _repo.Save(this);
        }
    }
}
