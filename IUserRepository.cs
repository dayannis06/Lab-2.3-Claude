using System.Threading.Tasks;

public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by their ID using parameterized queries to prevent SQL injection.
    /// </summary>
    /// <param name="userId">The user ID to retrieve</param>
    /// <returns>User data or null if not found</returns>
    Task<dynamic> GetUserByIdAsync(int userId);
}
