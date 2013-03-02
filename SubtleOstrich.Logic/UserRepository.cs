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
    }
}