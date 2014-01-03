using System;
using System.Collections.Generic;
using System.Linq;
using MongoRepository;

namespace SubtleOstrich.Logic
{
    public class UserRepository : IUserRepository
    {
        private readonly IRepository<User> _mongo;

        public UserRepository()
        {
            _mongo = new MongoRepository<User>();
        }

        public void Save(User entity)
        {
            _mongo.Update(entity);
        }

        public User GetBySourceAndId(string providerName, string id)
        {
            return _mongo.FirstOrDefault(u => u.Id == string.Format("{0}:{1}", providerName, id));
        }

        public IEnumerable<Activity> GetActivities(string id, DateTime date)
        {
            var activities = GetActivities(id);
            if (activities == null)
                return new List<Activity>();

            return activities.Where(act => act.Records != null && act.Records.Any(rec => rec.Date.ToLocalTime().ToShortDateString() == date.ToShortDateString()));
        }

        public IList<Activity> GetActivities(string id)
        {
            return _mongo.First(u => u.Id == id).Activities ?? new List<Activity>();
        }

        public string GetName(string id)
        {
            return GetById(id).Name;
        }

        public User GetById(string id)
        {
            return _mongo.GetById(id); 
        }
    }
}