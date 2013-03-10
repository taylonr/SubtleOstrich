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
            return _mongo.GetSingle(u => u.Id == string.Format("{0}:{1}", providerName, id));
        }

        public IEnumerable<Activity> GetActivities(string id, DateTime date)
        {
            var activities = _mongo.GetSingle(u => u.Id == id).Activities;
            return activities.Where(act => act.Records.Any(rec => rec.Date.ToShortDateString() == date.ToShortDateString()));
        }

        public User GetById(string id)
        {
            return _mongo.GetById(id); 
        }
    }
}