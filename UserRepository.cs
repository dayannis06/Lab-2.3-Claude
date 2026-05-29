using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

public class UserRepository : IUserRepository
{
    private readonly IConfiguration _configuration;

    public UserRepository(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<dynamic> GetUserByIdAsync(int userId)
    {
        try
        {
            // Get connection string from configuration (appsettings.json)
            var connectionString = _configuration.GetConnectionString("DefaultConnection") 
                ?? "Data Source=users.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = connection.CreateCommand())
                {
                    // Use parameterized query to prevent SQL injection
                    command.CommandText = "SELECT * FROM users WHERE user_id = @userId";
                    
                    // Add parameter safely - this escapes and prevents injection
                    command.Parameters.AddWithValue("@userId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            // Map reader results to an object
                            return new
                            {
                                UserId = reader["user_id"],
                                UserName = reader["user_name"],
                                Email = reader["email"],
                                // Add other columns as needed
                            };
                        }
                    }
                }
            }

            return null;
        }
        catch (SqliteException ex)
        {
            // Log the exception here using ILogger
            Console.Error.WriteLine($"Database error retrieving user {userId}: {ex.Message}");
            throw;
        }
    }
}
