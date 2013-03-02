using MongoRepository;

namespace SubtleOstrich.Logic
{
    public interface IUserRepository
    {
        void Save(User entity);
    }
}