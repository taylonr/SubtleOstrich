using System;
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

        public User GetByGuid(Guid identifier)
        {
            return _mongo.GetSingle(u => u.Identifier == identifier);
        }
    }
}