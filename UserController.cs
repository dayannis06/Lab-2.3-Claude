using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    [HttpGet("/user")]
    [Authorize] // Only authenticated users can access
    public async Task<IActionResult> GetUserProfile()
    {
        try
        {
            // Extract and validate input
            string userId = Request.Query["id"].ToString();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("User ID is required.");
            }

            // Validate that userId is a valid integer/GUID format
            if (!int.TryParse(userId, out int parsedUserId))
            {
                return BadRequest("User ID must be a valid number.");
            }

            // Verify authorization: ensure user can only access their own profile
            var currentUserId = User.FindFirst("sub")?.Value ?? User.Identity?.Name;
            if (parsedUserId.ToString() != currentUserId)
            {
                return Forbid("You can only access your own profile.");
            }

            // Use repository to fetch user data (parameterized query)
            var userData = await _userRepository.GetUserByIdAsync(parsedUserId);

            if (userData == null)
            {
                return NotFound("User not found.");
            }

            // Process and format data
            var processedData = ProcessData(userData);
            var profileResponse = FormatResponse(processedData);

            return Ok(profileResponse);
        }
        catch (Exception ex)
        {
            // Log the exception (use ILogger in production)
            Console.Error.WriteLine($"Error retrieving user profile: {ex.Message}");
            
            // Return generic error to client
            return StatusCode(500, "An error occurred while retrieving the user profile.");
        }
    }

    private object ProcessData(object userData)
    {
        // Implementation details...
        return userData;
    }

    private object FormatResponse(object data)
    {
        // Implementation details...
        return data;
    }
}
