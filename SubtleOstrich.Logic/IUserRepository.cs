using MongoRepository;

namespace SubtleOstrich.Logic
{
    public interface IUserRepository
    {
        void Save(User entity);
        User GetBySourceAndId(string providerName, string id);
    }
}