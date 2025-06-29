using BeerContest.Domain.Models;

namespace BeerContest.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<string> CreateAsync(User user);
        Task UpdateAsync(User user);
    }
}