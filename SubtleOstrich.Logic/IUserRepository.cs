using System;
using System.Collections.Generic;
using MongoRepository;

namespace SubtleOstrich.Logic
{
    public interface IUserRepository
    {
        void Save(User entity);
        User GetBySourceAndId(string providerName, string id);
        IEnumerable<Activity> GetActivities(string id, DateTime date);
        IList<Activity> GetActivities(string id);
        string GetName(string id);
    }
}