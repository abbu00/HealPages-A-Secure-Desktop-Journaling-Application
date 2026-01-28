using Microsoft.EntityFrameworkCore;

public class UserService
{
    private readonly Database _db;

    public UserService(Database dbContext)
    {
        _db = dbContext;
    }
    
    public async Task<User?> AuthenticateAsync(string loginName, string pin)
    {
        try
        {
            return await _db.User
                .FirstOrDefaultAsync(u => u.Username == loginName && u.PinCode == pin);

        }
        catch (Exception e)
        {
            Console.WriteLine("Something failed, maybe user already exists");
            return null;
        }
         }
    
    public async Task<(bool IsSuccessful, string? Message, User? CreatedUser)> CreateUserAsync(User account)
    {
        if (string.IsNullOrWhiteSpace(account.Username) ||
            string.IsNullOrWhiteSpace(account.PinCode))
        {
            return (false, "Username and PinCode are required.", null);
        }

        var alreadyExists = await _db.User
            .AnyAsync(u => u.Username == account.Username);

        if (alreadyExists)
        {
            return (false, "User already exists.", null);
        }

        var newUser = new User
        {
            Username = account.Username,
            PinCode = account.PinCode,
            Theme = string.IsNullOrWhiteSpace(account.Theme) ? "light" : account.Theme
        };

        _db.User.Add(newUser);
        await _db.SaveChangesAsync();

        try
        {
            return (true, null, newUser);

        }
        
        
            catch (Exception e)
        {
            Console.WriteLine("Something failed, maybe user already exists");
            return (true, null, newUser);
        }
    }
}